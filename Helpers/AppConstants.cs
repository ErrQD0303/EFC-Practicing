using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers
{
    public static class AppConstants
    {
        public static Dictionary<string, Dictionary<string, string[]>> MODEL_TYPE_MAPPINGS = new()
        {
            { "Product", new Dictionary<string, string[]>
                {
                    { "Price", new string[]
                        {
                            "Microsoft.EntityFrameworkCore.SqlServer:money",
                            "Npgsql.EntityFrameworkCore.PostgreSQL:money"
                        }
                    },
                    { "Name", new string[]
                        {
                            "Microsoft.EntityFrameworkCore.SqlServer:nvarchar(max)",
                            "Npgsql.EntityFrameworkCore.PostgreSQL:text"
                        }
                    }
                }
            },
            { "Category", new Dictionary<string, string[]>
                {
                    { "Description", new string[]
                        {
                            "Microsoft.EntityFrameworkCore.SqlServer:nvarchar(max)",
                            "Npgsql.EntityFrameworkCore.PostgreSQL:text"
                        }
                    }
                }
            }
        };
    }
}