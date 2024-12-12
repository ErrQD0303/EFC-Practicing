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
                    }
                }
            },
            { "Category", new Dictionary<string, string[]>
                {
                    { "Description", new string[]
                        {
                            "Microsoft.EntityFrameworkCore.SqlServer:ntext",
                            "Npgsql.EntityFrameworkCore.PostgreSQL:text"
                        }
                    }
                }
            }
        };
    }
}