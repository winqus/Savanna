using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Models
{
    public class Move
    {
        public int ColChange { get; set; }
        public int RowChange { get; set; }

        public Move()
        {
            // No logic
        }

        public Move(int rowChange, int colChange)
        {
            RowChange = rowChange;
            ColChange = colChange;
        }
    }
}
