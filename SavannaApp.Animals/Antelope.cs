using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SavannaApp.Models;

namespace SavannaApp.Animals
{
    public class Antelope : Animal
    {
        public Antelope()
        {
            Data.VisionRange = 3;
            Data.MaxHealth = 10;
            Data.Health = Data.MaxHealth;
        }

        public override Move? AttemptMove(AnimalFieldModel visionField)
        {
            if (MoveQueueEmpty())
            {
                Move? nextMove;

                nextMove = MoveToAvoidEnemies(visionField);
                if (nextMove == null)
                {
                    nextMove = AttemptRandomMove(visionField);
                }

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
            if (RandomSuccess(0.5) == false)
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

        public Move? MoveToAvoidEnemies(AnimalFieldModel visionField)
        {
            List<Tuple<int, Move>> enemyCountsInMoveDirections = new();

            bool seenEnemies = false;
            int offset = visionField.FieldLength / 2 - 1;
            int center = visionField.FieldLength / 2;
            int step = visionField.FieldLength / 2 - 1;
            int checkBlockLength = visionField.FieldLength / 2;
            for (int row_offset = -offset; row_offset <= offset; row_offset += step)
            {
                for (int col_offset = -offset; col_offset <= offset; col_offset += step)
                {
                    if (row_offset == 0 && col_offset == 0)
                    {
                        continue;
                    }
                    Animal? animal = visionField.Animals[center + Math.Clamp(row_offset, -1, 1), center + Math.Clamp(col_offset, -1, 1)];
                    if (animal == null)
                    {
                        int row_start = center + row_offset - step / 2;
                        int col_start = center + col_offset - step / 2;
                        int enemyCount = CountEnemies(visionField, row_start, row_start + checkBlockLength - 1, col_start, col_start + checkBlockLength - 1);

                        if (enemyCount > 0)
                        {
                            seenEnemies = true;
                        }

                        enemyCountsInMoveDirections.Add(new Tuple<int, Move>
                        (
                            enemyCount,
                            new Move(rowChange: Math.Clamp(row_offset, -1, 1), colChange: Math.Clamp(col_offset, -1, 1))
                        ));
                    }
                }
            }

            if(seenEnemies && enemyCountsInMoveDirections.Any())
            {
                int largestRangeWithoutEnemies = 0;
                int rangeStart = 0;
                for (int start = 0; start < enemyCountsInMoveDirections.Count; start++)
                {
                    int range = 0;
                    for (; range < enemyCountsInMoveDirections.Count; range++)
                    {
                        if (enemyCountsInMoveDirections[(start + range) % enemyCountsInMoveDirections.Count].Item1 != 0)
                        {
                            break;
                        }
                    }

                    if(range > largestRangeWithoutEnemies)
                    {
                        largestRangeWithoutEnemies = range;
                        rangeStart = start;
                    }
                }
                int index = ((largestRangeWithoutEnemies / 2) + rangeStart) % enemyCountsInMoveDirections.Count;
                return enemyCountsInMoveDirections[index].Item2;
            }

            return null;
        }

        public int CountEnemies(AnimalFieldModel visionField, int row_start, int row_end, int col_start, int col_end)
        {
            if(row_start < 0 || row_end >= visionField.FieldHeight || col_start < 0 || col_end >= visionField.FieldLength)
            {
                throw new ArgumentOutOfRangeException("Constraint values for row or col are out of range of field dimensions.");
            }

            int count = 0;
            for (int row = row_start; row <= row_end; row++)
            {
                for (int col = col_start; col <= col_end; col++)
                {
                    var animal = visionField.Animals[row, col];
                    if (animal != null && animal.GetType() == typeof(Lion))
                    {
                        count++;
                    }
                }
            }

            return count;
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
