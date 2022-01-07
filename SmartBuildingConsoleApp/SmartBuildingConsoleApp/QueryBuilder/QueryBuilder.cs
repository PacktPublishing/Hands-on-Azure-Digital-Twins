using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuildingConsoleApp.DigitalTwins
{
    public class QueryBuilder
    {
        private List<string> whereClauses = new List<string>();
        private List<string> joins = new List<string>();
        private string from = "";

        public string Query()
        {
            return "";
        }

        public void From(string twinId)
        {
            from = twinId != "" ? $"FROM DIGITALTWINS " + twinId : $"FROM DIGITALTWINS";
        }



    }
}
