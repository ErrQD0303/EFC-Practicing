using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFC_Attributes
{
    public class RepositoryTableAttribute : Attribute
    {
        public string TableName { get; }
        public RepositoryTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}