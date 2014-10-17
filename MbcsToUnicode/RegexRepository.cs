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
            public SearchPattern(Regex a, Regex b, string r)
            {
                begin = a;
                end = b;
                replace = r;
            }

            public Regex begin;
            public Regex end;
            public string replace;
        }

        private Dictionary<string, SearchPattern> _RegexDict;

        public RegexRepository()
        {
            _RegexDict = new Dictionary<string, SearchPattern>()
            {
                //http://stackoverflow.com/questions/406230/regular-expression-to-match-string-not-containing-a-word
                //{ " (\"", new Regex ("(([^_T])|(^((?!TRACE).)*))\\(\\\"")}//Example: ("
                { " (\"", new SearchPattern(new Regex ("[^_TRACE]\\(\\\""), new Regex ("[^\\\\]\""), "_T(\"{0}\")")   }//Example: ("
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

                            //Replace line
                            replaced_line = line.Substring(0, begin_pos)
                                + string.Format(entry.Value.replace, line.Substring(begin_pos + match_value.Length, end_pos - begin_pos - match_value.Length))
                                + line.Substring(end_pos, line.Length - end_pos);
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
