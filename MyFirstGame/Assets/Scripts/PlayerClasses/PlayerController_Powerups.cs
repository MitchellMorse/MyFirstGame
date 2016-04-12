using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;

public partial class PlayerController
{
    private const float _speedForceMultiplier = 2000;
    private List<Powerup> powerups;

    private void CheckForPowerupInput()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            HandlePowerupFired(false);
        }
    }

    private void HandlePowerupFired(bool isSecondary)
    {
        //TODO: once I have a mechanism to select powerups, will need to check which is selected.  For now speed is the only thing that can be selected
        HandleSpeedPrimary();
    }

    #region Generic Methods

    public int GetPowerUpCount(PowerupTypes type)
    {
        Powerup powerupOfType = powerups.Single(p => p.Type == type);
        return powerupOfType.TempCount + powerupOfType.PermanentCount;
    }

    public int GetTempPowerUpCount(PowerupTypes type)
    {
        Powerup powerupOfType = powerups.Single(p => p.Type == type);
        return powerupOfType.TempCount;
    }

    public int GetPermanentPowerUpCount(PowerupTypes type)
    {
        Powerup powerupOfType = powerups.Single(p => p.Type == type);
        return powerupOfType.PermanentCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns>true if there was a powerup available to decrement, false otherwise</returns>
    public bool DecrementPowerUpCount(PowerupTypes type)
    {
        bool success = false;
        Powerup powerupOfType = powerups.Single(p => p.Type == type);
        if (powerupOfType.TempCount > 0)
        {
            powerupOfType.TempCount -= 1;
            success = true;
        }
        else if (powerupOfType.PermanentCount > 0)
        {
            powerupOfType.PermanentCount -= 1;
            success = true;
        }

        return success;
    }

    public void IncrementTempPowerUpCount(PowerupTypes type)
    {
        Powerup powerupOfType = powerups.Single(p => p.Type == type);
        powerupOfType.TempCount += 1;
    }

    public void IncrementPermanentPowerUpCount(PowerupTypes type)
    {
        Powerup powerupOfType = powerups.Single(p => p.Type == type);
        powerupOfType.PermanentCount += 1;
    }

    public void InitializePowerupList(IPlayerStats stats)
    {
        powerups = new List<Powerup>();

        powerups.Add(new Powerup(PowerupTypes.Jump, stats.PermanentJumpCount, stats.TempJumpCount));
        powerups.Add(new Powerup(PowerupTypes.Speed, stats.PermanentSpeedCount, stats.TempSpeedCount));
    }
    #endregion

    #region Speed Powerup
    private void HandleSpeedPrimary()
    {
        if (moveHorizontal == 0 && moveVertical == 0) return;

        if (!DecrementPowerUpCount(PowerupTypes.Speed)) return;

        SendSpriteInNewDirection(moveHorizontal, moveVertical, _speedForceMultiplier);
    }
    #endregion
}
