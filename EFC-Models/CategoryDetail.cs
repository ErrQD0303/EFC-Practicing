using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EFC_Interfaces;

namespace EFC_Models
{
    public class CategoryDetail : IEFModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int CountProduct { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}