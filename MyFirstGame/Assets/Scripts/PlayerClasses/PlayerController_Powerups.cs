using UnityEngine;
using System.Collections;

public partial class PlayerController
{
    private const float _speedForceMultiplier = 2000;

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

    #region Speed Powerup
    private void HandleSpeedPrimary()
    {
        if (moveHorizontal == 0 && moveVertical == 0) return;

        if (_playerStats.TemporarySpeedCount == 0 && _playerStats.PermanentSpeedCount == 0) return;

        if (_playerStats.TemporarySpeedCount > 0)
        {
            _playerStats.TemporarySpeedCount--;
        }
        else if (_playerStats.PermanentSpeedCount > 0)
        {
            _playerStats.PermanentSpeedCount--;
        }

        StopVelocity();

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(movement * _speedForceMultiplier);
    }
    #endregion
}
