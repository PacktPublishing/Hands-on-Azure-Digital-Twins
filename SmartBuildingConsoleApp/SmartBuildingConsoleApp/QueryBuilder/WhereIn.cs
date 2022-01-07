using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuildingConsoleApp.QueryBuilder
{
    public class WhereIn : IQueryPart
    {
        public string parameter;
        public List<string> results;

        public WhereIn(string parameter, List<string> results)
        {
            this.parameter = parameter;
            this.results = results;
        }

        public string Result()
        {
            string inResult = "";

            foreach (string result in results)
            {
                if (inResult != "")
                {
                    inResult += $",";
                }
                inResult += $"'" + result + $"'";
            }

            return parameter + $" IN [" + inResult + "]";
        }
    }
}
