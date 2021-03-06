﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerClasses
{
    public partial class PlayerController
    {
        private const float SpeedForceMultiplier = 2000;
        private List<Powerup> Powerups;
        private PowerupTypes PrimaryPowerup;
        private PowerupTypes SecondaryPowerup;

        public GameObject PrimaryPowerupImage;
        public GameObject SecondaryPowerupImage;

        public void PrimaryPowerupClicked()
        {
            PrimaryPowerup = PrimaryPowerup == PowerupTypes.Jump ? PowerupTypes.Speed : PowerupTypes.Jump;

            SetPowerupHudImages();
        }

        public void SecondaryPowerupClicked()
        {
            SecondaryPowerup = SecondaryPowerup == PowerupTypes.Jump ? PowerupTypes.Speed : PowerupTypes.Jump;

            SetPowerupHudImages();
        }

        private void SetInitialPowerupSettings()
        {
            PrimaryPowerup = PowerupTypes.Jump;
            SecondaryPowerup = PowerupTypes.Speed;
        }

        private void CheckForPowerupInput()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                HandlePowerupFired(false);
            }
        }

        private void SetPowerupHudImages()
        {
            Image primaryImage = PrimaryPowerupImage.GetComponent<Image>();
            Image secondaryImage = SecondaryPowerupImage.GetComponent<Image>();

            primaryImage.sprite = _sprites.Single(s => s.name == string.Format("{0}Pickup", PrimaryPowerup.ToString()));
            secondaryImage.sprite = _sprites.Single(s => s.name == string.Format("{0}Pickup", SecondaryPowerup.ToString()));
        }

        private void HandlePowerupFired(bool isSecondary)
        {
            switch (PrimaryPowerup)
            {
                case PowerupTypes.Speed:
                    HandleSpeedPrimary();
                    break;
                case PowerupTypes.Jump:
                    HandleJumpPrimaryInitial();
                    break;
            }
        }

        #region Generic Methods

        public int GetPowerUpCount(PowerupTypes type)
        {
            Powerup powerupOfType = Powerups.Single(p => p.Type == type);
            return powerupOfType.TempCount + powerupOfType.PermanentCount;
        }

        public int GetTempPowerUpCount(PowerupTypes type)
        {
            Powerup powerupOfType = Powerups.Single(p => p.Type == type);
            return powerupOfType.TempCount;
        }

        public int GetPermanentPowerUpCount(PowerupTypes type)
        {
            Powerup powerupOfType = Powerups.Single(p => p.Type == type);
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
            Powerup powerupOfType = Powerups.Single(p => p.Type == type);
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
            Powerup powerupOfType = Powerups.Single(p => p.Type == type);
            powerupOfType.TempCount += 1;
        }

        public void IncrementPermanentPowerUpCount(PowerupTypes type)
        {
            Powerup powerupOfType = Powerups.Single(p => p.Type == type);
            powerupOfType.PermanentCount += 1;
        }

        public void InitializePowerupList(IPlayerStats stats)
        {
            Powerups = new List<Powerup>();

            Powerups.Add(new Powerup(PowerupTypes.Jump, stats.PermanentJumpCount, stats.TempJumpCount));
            Powerups.Add(new Powerup(PowerupTypes.Speed, stats.PermanentSpeedCount, stats.TempSpeedCount));
        }
        #endregion

        #region Speed Powerup
        private void HandleSpeedPrimary()
        {
            if (moveHorizontal == 0 && moveVertical == 0) return;

            if (!DecrementPowerUpCount(PowerupTypes.Speed)) return;

            SendSpriteInNewDirection(moveHorizontal, moveVertical, SpeedForceMultiplier);
        }

        private void HandleJumpPrimaryInitial()
        {
            if (!DecrementPowerUpCount(PowerupTypes.Jump)) return;

            Physics2D.IgnoreLayerCollision((int)SpriteLayers.Player, (int)SpriteLayers.Wall, true);
            Physics2D.IgnoreLayerCollision((int)SpriteLayers.Player, (int)SpriteLayers.BlockingLayer, true);
            CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.Airborne);
        }

        private void HandleJumpOngoing()
        {
            if (!CurrentState.CheckForExistenceOfBit((int) SpriteEffects.Airborne)) return;

            bool finished = GrowShrinkAnimation(_maxJumpScale, OriginalScale, AmountToScaleBy);

            if (finished)
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.Airborne);
                Physics2D.IgnoreLayerCollision((int)SpriteLayers.Player, (int)SpriteLayers.Wall, false);
                Physics2D.IgnoreLayerCollision((int)SpriteLayers.Player, (int)SpriteLayers.BlockingLayer, false);
            }
        }


        #endregion
    }
}
