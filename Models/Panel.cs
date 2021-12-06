namespace BattleShip.Domain
{
    public class Panel
    {
        public Panel(Coordinates coordinates)
        {
            Coordinates = coordinates;
            OccupationType = OccupationType.Empty;
            this.Ship = null;
        }

        public Coordinates Coordinates { get; private set; }

        public AbstractShip? Ship { get; private set; } = null;

        public OccupationType OccupationType { get; private set; }

        public void UpdateType(OccupationType newType)
        {
            OccupationType = newType;
        }

        public void AddShip(AbstractShip ship)
        {
            Ship = ship;
            OccupationType = OccupationType.Occupied;
        }

        public void RemoveShip()
        {
            this.Ship = null;
        }
    }
}
