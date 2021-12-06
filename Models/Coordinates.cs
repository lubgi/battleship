using System;

namespace BattleShip.Domain
{
    public struct Coordinates 
    {
        public Coordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public double Radius { get => Math.Sqrt(Row * Row + Column * Column); }

        public void ChangeCoordinatesDependingOnDirection(Direction direction)
        {
            _ = direction switch
            {
                Direction.North => Row++,
                Direction.South => Row--,
                Direction.East => Column++,
                Direction.West => Column--
            };
        }

        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.Row + b.Row, a.Column + b.Column);
        }

        public static Coordinates operator -(Coordinates a)
        { 
            return new Coordinates(-a.Row, -a.Column);
        }

        public static Coordinates operator -(Coordinates a, Coordinates b)
        {
            return a + (-b);
        }
    }
}
