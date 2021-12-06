namespace BattleShip.Domain
{
    class FighterShip : AbstractShip, IFighter
    {
        public override int MaxSpeed => base.MaxSpeed - 1;

        public FighterShip(
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
    }
}