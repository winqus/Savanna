using SavannaApp.Animals;
using SavannaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Animals
{
    public class Ghost : Animal
    {
        public Ghost()
        {
            Data.VisionRange = 0;
            Data.Health = 0;
            Data.IsAlive = false;
        }

        public override Move? AttemptMove(AnimalFieldModel visionField)
        {
            return null;
        }

        public override void ChangeHealth(double value)
        {
            return;
        }

        public override void Die()
        {
            return;
        }
    }
}
