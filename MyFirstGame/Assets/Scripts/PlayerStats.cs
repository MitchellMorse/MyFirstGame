using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class PlayerStats : MonoBehaviour
{

    private int _permanentSpeedCount;
    private int _temporarySpeedCount;

    public int PermanentSpeedCount 
    {
        get { return _permanentSpeedCount;}
        set { _permanentSpeedCount = value; }
    }

    public int TemporarySpeedCount
    {
        get { return _temporarySpeedCount; }
        set { _temporarySpeedCount = value; }
    }

    public PlayerStats()
    {
        TemporarySpeedCount = 5;
        PermanentSpeedCount = 5;
    }

    // Use this for initialization
    //void Start()
    //{
    //    TemporarySpeedCount = 5;
    //    PermanentSpeedCount = 5;
    //}

    //// Update is called once per frame
    //void Update () {

    //}
}
