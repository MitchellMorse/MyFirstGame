﻿using System;

namespace Assets.Scripts.Utilities
{
    [Flags]
    public enum SpriteEffects
    {
        Normal = 0,
        Shrinking = 1,
        Growing = 2,
        DownSlope = 4,
        UpSlope = 8,
        RightSlope = 16,
        LeftSlope = 32,
        Airborne = 64,
        MaxHeightReached = 128,
        Dead = 256,
        Invincible = 512,
        ControlledByOtherObject = 1024
    }

    [Flags]
    public enum PlayerEffects
    {
        Normal = 0,
        EndOfLevelReached = 1
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
        PickupSpeed,
        DamagingEnemy,
        LevelGoal
    }

    public enum PowerupTypes
    {
        Speed,
        Jump
    }

    public enum Layers
    {
        Default,
        Wall,
        Floor,
        Powerups,
        Player,
        Airborn
    }

    public enum SpriteLayers
    {
        BlockingLayer = 8,
        Powerup,
        Floor,
        Wall,
        Pickups,
        Player
    }
}