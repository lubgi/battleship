using System;

namespace BattleShip.Domain
{
    [Flags]
    public enum ActionType
    {
        None = 0,
        Attack = 1,
        Heal = 2,
        Move = 4
    }
}
