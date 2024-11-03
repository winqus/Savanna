using SavannaApp.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Models
{
    public class AnimalPair
    {
        public Animal Animal1 { get; set; }
        
        public Animal Animal2 { get; set; }

        public int PairFormedUpdate { get; set; }
    }
}
