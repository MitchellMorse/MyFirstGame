using System;

[Flags]
public enum SpriteState
{
    Normal = 0,
    Shrinking = 1,
    Growing = 2,
    DownSlope = 4,
    UpSlope = 8,
    RightSlope = 16,
    LeftSlope = 32
}

public enum Tags
{
    Respawn,
    Finish,
    EditorOnly,
    MainCamera,
    Player,
    GameController,
    Wall,
    Floor,
    FloorSlopeDown,
    FloorSlopeUp,
    FloorSlopeLeft,
    FloorSlopeRight,
    PickupSpeed
}

public enum PowerupTypes
{
    Speed,
    Jump
}