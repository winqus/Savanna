using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SavannaApp.Models;

namespace SavannaApp.Animals
{
    public class Lion : Animal
    {
        public TargetModel? Target { get; protected set; } = null;

        public bool HasTarget() => Target != null && Target.Animal.Data.IsAlive;

        public Lion()
        {
            Data.VisionRange = 2;
            Data.MaxHealth = 5;
            Data.Health = Data.MaxHealth;
        }

        public override Move? AttemptMove(AnimalFieldModel visionField)
        {
            if(MoveQueueEmpty())
            {
                Target = GetClosestTarget(visionField);
                if (HasTarget() == false)
                {
                    Move? nextMove = AttemptRandomMove(visionField);

                    if (nextMove != null)
                    {
                        Data.MoveQueue.Enqueue(nextMove);
                    }
                }
                else
                {
                    Move? nextMove;

                    nextMove = MoveToTarget(visionField);

                    if (nextMove != null)
                    {
                        Data.MoveQueue.Enqueue(nextMove);
                    }
                }
            }

            if(MoveQueueEmpty() == false)
            {
                ChangeHealth(-0.5);
                return Data.MoveQueue.Dequeue();
            }

            return null;
        }

        private TargetModel? GetClosestTarget(AnimalFieldModel visionField)
        {
            int center = visionField.FieldLength / 2;
            for (int offset = 1; offset <= this.Data.VisionRange; offset++)
            {
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
                            continue;
                        }

                        if (animal.GetType() == typeof(Antelope) && animal.Data.IsAlive)
                        {
                            return new TargetModel() 
                            {
                                Animal = animal,
                                RowOffset = row_offset,
                                ColOffset = col_offset
                            };
                        }
                    }
                }
            }
            return null;
        }

        protected Move? MoveToTarget(AnimalFieldModel visionField)
        {
            if(HasTarget())
            {
                int center = visionField.FieldLength / 2;
                int rowChange, colChange;
                if (Target.RowOffset == 0 || Target.ColOffset == 0)
                {
                    rowChange = Target.RowOffset == 0 ? 0 : (Target.RowOffset <= -1 ? -1 : 1);
                    colChange = Target.ColOffset == 0 ? 0 : (Target.ColOffset <= -1 ? -1 : 1);
                    var animalAtPosition = visionField.Animals[center + rowChange, center + colChange];
                    if (animalAtPosition == null || animalAtPosition.GetType() == typeof(Antelope))
                    {
                        return new Move(rowChange, colChange);
                    }
                }
                else
                {
                    rowChange = Target.RowOffset <= -1 ? -1 : 1;
                    colChange = Target.ColOffset <= -1 ? -1 : 1;
                    List<Move> possibleMoves = new List<Move>() 
                    { 
                        new Move(rowChange, colChange),
                        new Move(rowChange, 0),
                        new Move(0, colChange)
                    };

                    foreach(var moveAttempt in possibleMoves)
                    {
                        var animalAtPosition = visionField.Animals[center + moveAttempt.RowChange, center + moveAttempt.ColChange];
                        if (animalAtPosition == null || animalAtPosition.GetType() == typeof(Antelope))
                        {
                            return new Move(rowChange, colChange);
                        }
                    }
                }
            }

            return null;
        }

        protected Queue<Move> GenerateMovesToTarget(AnimalFieldModel visionField)
        {
            var moves = new Queue<Move>();

            if (HasTarget() == false)
            {
                return moves;
            }
            


            return moves;
        }

        protected Move? AttemptRandomMove(AnimalFieldModel visionField)
        {
            if(RandomSuccess(0.5) == false)
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

            if(possibleMoves.Any())
            {
                return possibleMoves[(new Random()).Next(0, possibleMoves.Count)];
            }

            return null;
        }

        public override void ChangeHealth(double value)
        {
            Data.Health = Math.Clamp(Data.Health + value, 0, Data.MaxHealth);

            if(Data.Health <= 0)
            {
                Die();
            }
        }

        public override void Die()
        {
            Data.IsAlive = false;
        }

        public static bool RandomSuccess(double successProbability)
        {
            Random random = new();
            double probability = random.NextDouble();

            return probability <= successProbability;
        }
    }
}
