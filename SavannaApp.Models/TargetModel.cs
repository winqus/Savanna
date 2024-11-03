using SavannaApp.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Models
{
    public class TargetModel
    {
        public Animal Animal { get; set; }

        public int RowOffset { get; set; }

        public int ColOffset { get; set; }
    }
}
