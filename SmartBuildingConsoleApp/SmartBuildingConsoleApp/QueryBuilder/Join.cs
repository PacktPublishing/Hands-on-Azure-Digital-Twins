using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuildingConsoleApp.QueryBuilder
{
    public class Join : IQueryPart
    {
        public string parameter;
        public string related;
        public string alias;

        public Join(string parameter, string related, string alias = "")
        {
            this.parameter = parameter;
            this.related = related;
            this.alias = alias;
        }

        public string Result()
        {
            return parameter + $" RELATED " + related + $" " + alias;
        }
    }
}
