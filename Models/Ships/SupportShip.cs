namespace BattleShip.Domain
{
    class SupportShip : AbstractShip, ISupport
    {
        public SupportShip(
            int maxSpeed,
            int length,
            int range,
            ShipType shipType,
            Coordinates coordinates,
            Direction direction) :
                      base(maxSpeed, length, range, shipType, coordinates, direction)
        {
        }

        public void Heal(Coordinates coordinates)
        {
            if (!this.CanPerformAction(coordinates))
            {
                return;
            }
            //// dosmth
        }
    }
}
