namespace BattleShip.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class AbstractShip
    {
        public AbstractShip(int maxSpeed,
                            int length,
                            int range,
                            ShipType shipType,
                            Coordinates coordinates,
                            Direction direction)
        {
            MaxSpeed = maxSpeed;
            Length = length;
            Range = range;
            Type = shipType;
            Coordinates = coordinates;
            Direction = direction;
        }

        public virtual int MaxSpeed { get; private set; }

        public int Length { get; private set; }

        public int Range { get; private set; }

        public ShipType Type { get; private set; }

        public Coordinates Coordinates { get; private set; }

        public Direction Direction { get; private set; }

        public int Hits { get; private set; } = 0;

        public void MoveTo(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public void GetsDamage()
        {
            Hits++;
        }

        public Coordinates GetClosestPanelCoordinates(Coordinates centerCoordinates)
        {
            var localCoordinates = Coordinates;
            var coordinatesList = new List<Coordinates>();

            foreach (var value in Enumerable.Range(1, Length))
            {
                coordinatesList.Add(localCoordinates);
                localCoordinates.ChangeCoordinatesDependingOnDirection(Direction);
            }

            return coordinatesList.OrderBy(e => (centerCoordinates - e).Radius).First();
        }
        public void GetsHeal()
        {
            Hits = Hits <= 0 ? 0 : --Hits;
        }

        public bool CanPerformAction(Coordinates coordinates, ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Move   => CanMoveTo(coordinates),
                ActionType.Attack => CanPerformAction(coordinates),
                ActionType.Heal   => CanPerformAction(coordinates)
            };
        }
        protected bool CanMoveTo(Coordinates coordinates)
        {
            return IsInRange(coordinates, MaxSpeed);
        }
        protected bool CanPerformAction(Coordinates coordinates)
        {
            return IsInRange(coordinates, Range);
        }

        private bool IsInRange(Coordinates destinationCoordinates, int range)
        {
            var columnRange = GetColumnCoordinatesRange(range);
            var rowRange    = GetRowCoordinatesRange(range);

            return columnRange.Contains(destinationCoordinates.Column) && rowRange.Contains(destinationCoordinates.Row);
        }

        private IEnumerable<int> GetColumnCoordinatesRange(int range)
        {
            var minShipCoordinatesColumn = GetMinCoordinates().Column;
            int start = minShipCoordinatesColumn - range;
            int count = Length + range * 2;

            if (Direction != Direction.East)
            {
                count = range * 2 + 1;
            }

            var columnRange = Enumerable.Range(start, count);
            return columnRange;
        }
        private IEnumerable<int> GetRowCoordinatesRange(int range)
        {
            var minShipCoordinatesRow = GetMinCoordinates().Row;
            int start = minShipCoordinatesRow - range;
            int count = Length + range * 2;

            if (Direction != Direction.North)
            {
                count = range * 2 + 1;
            }

            var rowRange = Enumerable.Range(start, count);
            return rowRange;
        }

        private Coordinates GetMinCoordinates()
        {
            return Direction switch
            {
                Direction.West => new Coordinates(Coordinates.Row, Coordinates.Column - Length),
                Direction.South => new Coordinates(Coordinates.Row - Length, Coordinates.Column),
                _ => this.Coordinates
            };
        }

    }
}
