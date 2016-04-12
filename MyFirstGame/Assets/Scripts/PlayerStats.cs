﻿using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

[Serializable]
public class PlayerStats : MonoBehaviour, IPlayerStats
{

    public int PermanentSpeedCount;
    public int TempSpeedCount;

    public int TempJumpCount;
    public int PermanentJumpCount;

    int IPlayerStats.PermanentSpeedCount
    {
        get { return PermanentSpeedCount; }

        set { PermanentSpeedCount = value; }
    }

    int IPlayerStats.TempSpeedCount
    {
        get { return TempSpeedCount; }

        set { TempSpeedCount = value; }
    }

    int IPlayerStats.PermanentJumpCount
    {
        get { return PermanentJumpCount; }

        set { PermanentJumpCount = value; }
    }

    int IPlayerStats.TempJumpCount
    {
        get { return TempJumpCount; }

        set { TempJumpCount = value; }
    }

    public PlayerStats()
    {
        TempSpeedCount = 5;
        PermanentSpeedCount = 5;

        TempJumpCount = 0;
        PermanentJumpCount = 0;
    }
}
