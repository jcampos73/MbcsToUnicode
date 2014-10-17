using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace MbcsToUnicode
{
    public class RegexRepository
    {
        private class SearchPattern
        {
            public SearchPattern(Regex a, Regex b, string r, string a_del, string b_del)
            {
                begin = a;
                end = b;
                replace = r;
                beginDelimiter = a_del;
                endDelimiter = b_del;
            }

            public Regex begin;//Regex to search for
            public Regex end;//Regex to find to check the ending
            public string replace;//Replace expression
            public string beginDelimiter;
            public string endDelimiter;
        }

        private Dictionary<string, SearchPattern> _RegexDict;

        public RegexRepository()
        {
            _RegexDict = new Dictionary<string, SearchPattern>()
            {
                //http://stackoverflow.com/questions/406230/regular-expression-to-match-string-not-containing-a-word
                //{ " (\"", new Regex ("(([^_T])|(^((?!TRACE).)*))\\(\\\"")}//Example: ("
                { " (\"", new SearchPattern(new Regex ("[^_TRACE]\\(\\\""), new Regex ("[^\\\\]\""), "_T({0})", "\"", "\"")  }//Example: ("
            };
        }

        public List<string> ProcessFileByLine(string[] lines)
        {
            int counter = 1;
            List<string> result = new List<string>();

            foreach(string line in lines)
            {

                foreach (KeyValuePair<string, SearchPattern> entry in _RegexDict)
                {
                    if (entry.Value.begin.Match(line).Success)
                    {
                        //Local variables
                        string replaced_line = "NOT REPLACED!";
                        string match_value = entry.Value.begin.Match(line).Value;
                        int begin_pos = entry.Value.begin.Match(line).Index;

                        //Match end regular Expression
                        if (entry.Value.end.Match(line, begin_pos + match_value.Length).Success)
                        {
                            //Local variables
                            string match_value_end = entry.Value.end.Match(line, begin_pos + match_value.Length).Value;
                            int end_pos = entry.Value.end.Match(line, begin_pos + match_value.Length).Index;

                            //Replaced part
                            string a1 = match_value.Substring(0, match_value.IndexOf(entry.Value.beginDelimiter));
                            string a2 = match_value.Substring(match_value.IndexOf(entry.Value.beginDelimiter), match_value.Length - match_value.IndexOf(entry.Value.beginDelimiter));
                            string b1 = match_value_end.Substring(0, match_value_end.IndexOf(entry.Value.endDelimiter));
                            string b2 = match_value_end.Substring(match_value_end.IndexOf(entry.Value.endDelimiter), match_value_end.Length - match_value_end.IndexOf(entry.Value.endDelimiter));

                            string replaced_part = string.Format(entry.Value.replace, a2
                                + line.Substring(begin_pos + match_value.Length, end_pos - begin_pos - match_value.Length)
                                + b1 + b2
                                );

                            //Replace line
                            replaced_line = line.Substring(0, begin_pos) + a1
                                + replaced_part
                                + line.Substring(end_pos + match_value_end.Length, line.Length - end_pos - match_value_end.Length);
                        }

                        //Construct feedback
                        string result_line = string.Format("{0}##{1}##( {2} )==>{3}==>{4}", counter, entry.Key, match_value, line.Trim(), replaced_line.Trim());
                        result.Add(result_line);

                        //Reverse string demo code
                        //string reverseValue = new string(line.Reverse().ToArray());
                    }
                }

                counter++;
            }

            return result;
        }
    }
}
