using SavannaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Animals
{
    public class Fox : Animal
    {
        public Fox()
        {
            Data.VisionRange = 3;
            Data.MaxHealth = 6;
            Data.Health = Data.MaxHealth;
        }

        public override Move? AttemptMove(AnimalFieldModel visionField)
        {
            if (MoveQueueEmpty())
            {
                Move? nextMove = AttemptRandomMove(visionField);

                if (nextMove != null)
                {
                    Data.MoveQueue.Enqueue(nextMove);
                }

            }

            if (MoveQueueEmpty() == false)
            {
                ChangeHealth(-0.5);
                return Data.MoveQueue.Dequeue();
            }

            return null;
        }

        protected Move? AttemptRandomMove(AnimalFieldModel visionField)
        {
            if (RandomSuccess(0.8) == false)
            {
                return null;
            }

            List<Move> possibleMoves = new();

            int offset = 1;
            int center = visionField.FieldLength / 2;
            for (int row_offset = -offset; row_offset <= offset; row_offset++)
            {
                for (int col_offset = -offset; col_offset <= offset; col_offset++)
                {
                    if (row_offset == 0 && col_offset == 0)
                    {
                        continue;
                    }

                    Animal? animal = visionField.Animals[center + row_offset, center + col_offset];
                    if (animal == null)
                    {
                        possibleMoves.Add(new Move(rowChange: row_offset, colChange: col_offset));
                    }
                }
            }

            if (possibleMoves.Any())
            {
                return possibleMoves[(new Random()).Next(0, possibleMoves.Count)];
            }

            return null;
        }
        public static bool RandomSuccess(double successProbability)
        {
            Random random = new();
            double probability = random.NextDouble();

            return probability <= successProbability;
        }

        public override void ChangeHealth(double value)
        {
            Data.Health = Math.Clamp(Data.Health + value, 0, Data.MaxHealth);

            if (Data.Health <= 0)
            {
                Die();
            }
        }

        public override void Die()
        {
            Data.IsAlive = false;
        }
    }
}
