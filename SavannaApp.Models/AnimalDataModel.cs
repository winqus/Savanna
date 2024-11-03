using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Models
{
    public class AnimalDataModel
    {
        public int VisionRange { get; set; }

        public double Health { get; set; }

        public double MaxHealth { get; set; }

        public bool IsAlive { get; set; }

        public Queue<Move> MoveQueue { get; set; }
    }
}
