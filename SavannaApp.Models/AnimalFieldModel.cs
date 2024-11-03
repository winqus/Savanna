using SavannaApp.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Models
{
    public class AnimalFieldModel
    {
        public Animal?[,] Animals { get; set; }

        public int FieldLength { get; set; }

        public int FieldHeight { get; set; }

        public int Updates { get; set; }
    }
}
