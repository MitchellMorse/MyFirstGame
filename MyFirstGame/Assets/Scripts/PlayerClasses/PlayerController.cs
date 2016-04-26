using System.Collections.Generic;
using Assets.Scripts.Abstract;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;
using SpriteState = UnityEngine.UI.SpriteState;

namespace Assets.Scripts.PlayerClasses
{
    public partial class PlayerController : GenericSprite
    {
        public Text fuelText;
        public int fuelCount;
        public int fuelCountDecrementMax;
        public Text primaryPowerupText;
        public Text secondoryPowerupText;

        private List<Sprite> _sprites;
        private float _powerSpeed = 20f;
        private int _fuelCountDecrement;
        private float _currenDecelTime = 0;
        private float _maxJumpScale;

        protected override void Start()
        {
            base.Start();
        
            _fuelCountDecrement = 0;
            InitializePowerupList(new PlayerStats());
            SetInitialPowerupSettings();
            moveVertical = moveHorizontal = 0;
            _maxJumpScale = transform.localScale.x*2;
            LoadSprites();

            SetPowerupHudImages();
        }

        private void LoadSprites()
        {
            _sprites = new List<Sprite>();
            _sprites.Add(Resources.Load<Sprite>("Sprites/Pickups/JumpPickup"));
            _sprites.Add(Resources.Load<Sprite>("Sprites/Pickups/SpeedPickup"));
        }

        protected override void Update()
        {
            base.Update();
        
            CheckForPlayerInput();

            AddForce();
            HandleJumpOngoing();
            UpdateHud();

            moveVertical = moveHorizontal = 0;
        }

        private void UpdateHud()
        {
            UpdateFuelCount(moveVertical, moveHorizontal);

            primaryPowerupText.text = string.Format("{0}", GetPowerUpCount(PrimaryPowerup));
            secondoryPowerupText.text = string.Format("{0}", GetPowerUpCount(SecondaryPowerup));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveVertical"></param>
        /// <param name="moveHorizontal"></param>
        /// <returns>true if any input is found</returns>
        private bool CheckForPlayerInput()
        {
            moveHorizontal = moveVertical = 0f;

            if (fuelCount > 0 && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                CheckForMovementInput(ref moveVertical, ref moveHorizontal);
            }

            CheckForPowerupInput();

            return moveVertical != 0 || moveHorizontal != 0;
        }

        private void CheckForMovementInput(ref float moveVertical, ref float moveHorizontal)
        {
            moveHorizontal = CurrentState.CheckForExistenceOfBit((int) SpriteEffects.Shrinking)
                ? 0
                : Input.GetAxis("Horizontal") + RightForce - LeftForce;
            moveVertical = CurrentState.CheckForExistenceOfBit((int) SpriteEffects.Shrinking)
                ? 0
                : Input.GetAxis("Vertical") - DownwardForce + UpwardForce;
        }

        private void UpdateFuelCount(float moveVertical, float moveHorizontal)
        {
            bool playerMoving = moveVertical != 0 || moveHorizontal != 0;

            if (playerMoving)
            {
                _fuelCountDecrement += 1;
            }

            if (_fuelCountDecrement >= fuelCountDecrementMax)
            {
                fuelCount -= 1;
                _fuelCountDecrement = 0;
            }

            SetDebugText(string.Format("{0}", fuelCount));
        }

        public bool IsPlayerAtPowerSpeed()
        {
            return rb2d.velocity.magnitude >= _powerSpeed;
        }

        void OnCollisionEnter2D(Collision2D collider)
        {
            if (collider.gameObject.CompareTag(Tags.DamagingEnemy.ToString()))
            {
                StopVelocity();

                Vector2 dir = (Vector3)collider.contacts[0].point - transform.position;
                dir = -dir.normalized;

                moveHorizontal = dir.x;
                moveVertical = dir.y;
                AddForce(1000);
            }
        }
    
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (other.gameObject.CompareTag(Tags.PickupSpeed.ToString()) && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                other.gameObject.SetActive(false);
                IncrementTempPowerUpCount(PowerupTypes.Speed);
            }
        }

        private void SetDebugText(string text)
        {
            fuelText.text = text;
        }
    }
}
