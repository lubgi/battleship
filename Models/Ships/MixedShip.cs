namespace BattleShip.Domain
{
    class MixedShip : AbstractShip, ISupport, IFighter
    {
        public override int MaxSpeed => base.MaxSpeed - 1;

        public MixedShip(
            int maxSpeed, 
            int length, 
            int range,
            ShipType shipType, 
            Coordinates coordinates,
            Direction direction) :
                   
            base(maxSpeed, length, range, shipType, coordinates, direction)
        { 
        }

        public void Attack(Coordinates coordinates)
        {
            if (!this.CanPerformAction(coordinates))
            {
                return;
            }
            //// dosmth
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
