using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class Spirit : ITarget
    {
        private int _vitality;
        public int Vitality { get => _vitality; set => _vitality = Math.Max(Math.Min(MaxVitality, value),0); }
        public int MaxVitality { get; set; }

        public int Strength { get; set; }
        public int Movement { get; set; }

        public Guid Id { get; private set; }
        public int TeamId { get; private set; }
        
        private Battle Battle { get; set; }

        public Point GridLocation { get; set; }

        public Spirit(Battle battle, int teamId, int x, int y)
        {
            Id = Guid.NewGuid();
            Battle = battle;
            TeamId = teamId;
            GridLocation = new Point(x, y);
            Movement = 1;
            battle.Grid[x, y].Spirit = this;
        }

        public int TakeDamage(int damage)
        {
            Vitality -= damage;
            return damage;
        }
        public ApproachPath GetApproachPath(Spirit enemy)
        {
            var adjacentSquares = new List<Point>()
            {
                enemy.GridLocation + new Point(0,1),
                enemy.GridLocation + new Point(0,-1),
                enemy.GridLocation + new Point(1,0),
                enemy.GridLocation + new Point(-1,0),
            };

            adjacentSquares = adjacentSquares.Where(p => p.X >= 0 && p.X <= Battle.GRID_MAX_X && p.Y >= 0 && p.Y <= Battle.GRID_MAX_Y).ToList(); //Filter illegal squares

            var closestSquare = adjacentSquares.OrderBy(p => AbsVector(p - GridLocation)).FirstOrDefault();

            if (closestSquare == default)
            {
                return null; //No valid squares?
            }

            var currentPosition = new Point(GridLocation.X, GridLocation.Y);
            var moveAvailable = Movement;
            var path = new ApproachPath();

            while (currentPosition != closestSquare && moveAvailable > 0)
            {
                if (currentPosition.X > closestSquare.X)
                {
                    path.Movements.Add(new Point(-1, 0));
                    currentPosition += new Point(-1, 0);
                }
                else if (currentPosition.X < closestSquare.X)
                {
                    path.Movements.Add(new Point(1, 0));
                    currentPosition += new Point(1, 0);
                }
                else if (currentPosition.Y > closestSquare.Y)
                {
                    path.Movements.Add(new Point(0, -1));
                    currentPosition += new Point(0, -1);
                }
                else if (currentPosition.Y < closestSquare.Y)
                {
                    path.Movements.Add(new Point(0, 1));
                    currentPosition += new Point(0, 1);
                }
                moveAvailable--;
            }
            path.Target = currentPosition;
            if (currentPosition == closestSquare)
            {
                path.AtTarget = true;
            }
            return path;
        }

        internal bool InRangeOf(Spirit target)
        {
            return AbsVector(GridLocation - target.GridLocation) == 1;
        }

        private int AbsVector(Point p)
        {
            return Math.Abs(p.X) + Math.Abs(p.Y);
        }
    }
}
