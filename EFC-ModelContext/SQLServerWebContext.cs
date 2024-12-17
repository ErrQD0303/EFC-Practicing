using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC_Attributes;
using EFC_Interfaces;
using EFC_Logger;
using EFC_Models.WebModels;
using Helpers;
using Microsoft.EntityFrameworkCore;

namespace EFC_ModelContext
{
    public class SQLServerWebContext : WebContext<SQLServerWebContext>
    {
        public SQLServerWebContext(DbContextOptions<SQLServerWebContext> options) : base(options) { }
    }
}