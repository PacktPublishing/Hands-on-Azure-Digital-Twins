using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuildingConsoleApp.QueryBuilder
{
    public class IsOfModel : IQueryPart
    {
        public string twinCollection;
        public bool exactMatch;
        public string twinTypeName;

        public IsOfModel(string twinCollection, string twinTypeName, bool exactMatch = false)
        {
            this.twinCollection = twinCollection;
            this.twinTypeName = twinTypeName;
            this.exactMatch = exactMatch;
        }

        public IsOfModel(string twinTypeName, bool exactMatch = false)
        {
            this.twinTypeName = twinTypeName;
            this.exactMatch = exactMatch;
        }

        public string Result()
        {
            if (twinCollection == "")
            {
                return exactMatch ? $"IS_OF_MODEL('" + twinTypeName + $"', exact)" : $"IS_OF_MODEL('" + twinTypeName + "')";
            }
            else
            {
                return exactMatch ? $"IS_OF_MODEL('" + twinCollection + "','" + twinTypeName + $"', exact)" : $"IS_OF_MODEL('" + twinCollection + "','" + twinTypeName + $"')";
            }
        }
    }
}
