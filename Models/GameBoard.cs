namespace BattleShip.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    
    public class GameBoard
    {
        private GameBoard(int height, int width, Coordinates center)
        {
            Height = height;
            Width = width;
            Ships = new List<AbstractShip>();
            Panels = CreatePanels(height, width);
            Center = center;
        }

        public List<AbstractShip> Ships { get; private set; }

        public Panel[][] Panels { get; private set; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public Coordinates Center { get; private set; }

        public AbstractShip this[Coordinates coordinates]
        {
            get => Panels[coordinates.Row][coordinates.Column].Ship;
        }

        public static GameBoard? CreateGameboard(int height, int width, Coordinates center)
        {
            if (height <= 0 || width <= 0 || AreOutOfBounds(height, width, center))
            {
                return null;
            }

            return new GameBoard(height, width, center);
        }

        public IReadOnlyList<object> GetState()
        {
            return 
                Ships
                .OrderBy(e => (Center - e.GetClosestPanelCoordinates(Center)).Radius)
                .ThenBy(e => e.Range)
                .Select(e => 
                new { 
                    ShipType    = e.Type, 
                    Length      = e.Length, 
                    Coordinates = e.Coordinates, 
                    Range       = e.Range, 
                    Hits        = e.Hits 
                })
                .Cast<object>().ToList();
        }

        public void AddShip(int maxSpeed, int length, int range, ShipType shipType, Coordinates coordinates, Direction direction)
        {
            if (!Enum.IsDefined(typeof(Direction), direction) ||
                !CheckIfPanelsAreEmpty(coordinates, direction, length) ||
                AreOutOfBounds(coordinates) ||
                !Enum.IsDefined(typeof(ShipType), shipType))
            {
                return;
            }

            AbstractShip newShip = shipType switch
            {
                ShipType.Fighter => new FighterShip(maxSpeed, length, range, shipType, coordinates, direction),
                ShipType.Support => new SupportShip(maxSpeed, length, range, shipType, coordinates, direction),
                ShipType.Mixed => new MixedShip(maxSpeed, length, range, shipType, coordinates, direction)
            };
            Ships.Add(newShip);

            foreach (var value in Enumerable.Range(1, newShip.Length))
            {
                var panel = GetPanel(coordinates);
                panel.AddShip(newShip);
                coordinates.ChangeCoordinatesDependingOnDirection(direction);
            }
        }

        public void PerformAction(Coordinates sourceCoordinates, ActionType actionType, Coordinates destinationCoordinates)
        {
            if (AreOutOfBounds(sourceCoordinates) || AreOutOfBounds(destinationCoordinates))
            {
                return;
            }

            var ship = this[sourceCoordinates];

            if (ship is null)
            {
                return;
            }

            (actionType switch
            {
                ActionType.Attack => (Action<Coordinates, AbstractShip>)AttackPanel,
                ActionType.Heal   => (Action<Coordinates, AbstractShip>)HealPanel,
                ActionType.Move   => (Action<Coordinates, AbstractShip>)MoveShip,
            })(destinationCoordinates, ship);

        }
        private Panel[][] CreatePanels(int height, int width)
        {
            var panels = new Panel[height][];

            for (int row = 0; row < height; row++)
            {
                panels[row] = new Panel[width];

                for (int column = 0; column < width; column++)
                {
                    panels[row][column] = new Panel(new Coordinates(row, column));
                }

            }

            return panels;
        }
        
        private void MoveShip(Coordinates destinationCoordinates, AbstractShip ship)
        {
            throw new NotImplementedException();
        }

        private void AttackPanel(Coordinates coordinates, AbstractShip attackerShip)
        {
            var panel = GetPanel(coordinates);
            if (panel.OccupationType != OccupationType.Occupied ||
                attackerShip as IFighter is null ||
                !attackerShip.CanPerformAction(coordinates, ActionType.Attack))
            {
                return;
            }

            panel.UpdateType(OccupationType.Damaged);
            var victimShip = panel.Ship;
            victimShip.GetsDamage();
            if (victimShip.Hits == victimShip.Length)
            {
                DeleteShip(victimShip);
            }

            ((IFighter)attackerShip).Attack(coordinates);

        }
        private void HealPanel(Coordinates coordinates, AbstractShip healerShip)
        {
            var panel = GetPanel(coordinates);

            if (panel.OccupationType != OccupationType.Damaged || healerShip as ISupport is null || !healerShip.CanPerformAction(coordinates, ActionType.Heal))
            {
                return;
            }

            panel.UpdateType(OccupationType.Occupied);
            var healedShip = panel.Ship;
            healedShip.GetsHeal();
            ((ISupport)healerShip).Heal(coordinates);
        }
        
        private void DeleteShip(AbstractShip shipToDelete)
        {
            var startingCoordinates = shipToDelete.Coordinates;

            foreach (var value in Enumerable.Range(1, shipToDelete.Length))
            {
                var panelToFree = GetPanel(startingCoordinates);
                panelToFree.RemoveShip();
                panelToFree.UpdateType(OccupationType.Empty);
                startingCoordinates.ChangeCoordinatesDependingOnDirection(shipToDelete.Direction);
            }

            Ships.Remove(shipToDelete);
        }

        private bool CheckIfPanelsAreEmpty(Coordinates startCoordinates, Direction direction, int length)
        {
            foreach (var value in Enumerable.Range(1, length))
            {
                if (AreOutOfBounds(startCoordinates) || 
                    GetPanel(startCoordinates).OccupationType != OccupationType.Empty)
                {
                    return false;
                }

                startCoordinates.ChangeCoordinatesDependingOnDirection(direction);
            }

            return true;
        }

        private bool AreOutOfBounds(Coordinates coordinates)
        {
            var row = coordinates.Row;
            var column = coordinates.Column;
            return !(row < Height && row >= 0 && column >= 0 && column < Width);
        }

        private static bool AreOutOfBounds(int height, int width, Coordinates coordinates)
        {
            var row = coordinates.Row;
            var column = coordinates.Column;
            return !(row < height && row >= 0 && column >= 0 && column < width);
        }

        private Panel GetPanel(Coordinates coordinates)
        {
            return Panels[coordinates.Row][coordinates.Column];
        }

    }
}
