using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SavannaApp.Models;

namespace SavannaApp.Animals
{
    public abstract class Animal
    {
        public AnimalDataModel Data { get; set; }

        public bool MoveQueueEmpty() => Data.MoveQueue.Any() == false;

        public Animal()
        {
            Data = new AnimalDataModel()
            {
                IsAlive = true,
                VisionRange = 2,
                MoveQueue = new()
            };
        }
        public abstract Move? AttemptMove(AnimalFieldModel visionField);

        public abstract void ChangeHealth(double value);

        public abstract void Die();
    }
}
