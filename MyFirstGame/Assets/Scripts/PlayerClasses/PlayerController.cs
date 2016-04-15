using Assets.Scripts.Abstract;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;
using SpriteState = UnityEngine.UI.SpriteState;

namespace Assets.Scripts.PlayerClasses
{
    public partial class PlayerController : GenericSprite
    {
        public float speed;
        public Text debugText;
        public int fuelCount;
        public int fuelCountDecrementMax;
        public Text primaryPowerupText;
        public Text secondoryPowerupText;
    
        private int TouchingFloorObjects;
        private float CurrentAcceleration;
        private float PowerSpeed = 20f;
        private int FuelCountDecrement;
        private float decelTime = 10;
        private float currenDecelTime = 0;
        private Vector3 testSpeed;
        private float moveVertical;
        private float moveHorizontal;
        private float MaxJumpScale;

        protected override void Start()
        {
            base.Start();
        
            TouchingFloorObjects = 0;
            CurrentAcceleration = 0f;
            FuelCountDecrement = 0;
            InitializePowerupList(new PlayerStats());
            SetInitialPowerupSettings();
            moveVertical = moveHorizontal = 0;
            MaxJumpScale = transform.localScale.x*2;
        }

        protected override void Update()
        {
            base.Update();
        
            CheckForPlayerInput();

            Vector2 movement = new Vector2(moveHorizontal, moveVertical); //- vertical goes down

            if (moveHorizontal != 0f || moveVertical != 0f)
            {
                rb2d.AddForce(movement*speed);
            }

            PlayerSpeedProcessing();
            HandlePlayerLeavingFloor();
            HandleJumpOngoing();
            UpdateHud();

            moveVertical = moveHorizontal = 0;
        }

        private void UpdateHud()
        {
            UpdateFuelCount(moveVertical, moveHorizontal);

            primaryPowerupText.text = string.Format("{1} Count: {0}",
                GetPowerUpCount(PrimaryPowerup), PrimaryPowerup.ToString());

            secondoryPowerupText.text = string.Format("{1} Count: {0}",
                GetPowerUpCount(SecondaryPowerup), SecondaryPowerup.ToString());
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
                FuelCountDecrement += 1;
            }

            if (FuelCountDecrement >= fuelCountDecrementMax)
            {
                fuelCount -= 1;
                FuelCountDecrement = 0;
            }

            SetDebugText(string.Format("Current fuel: {0}", fuelCount));
        }

        private void PlayerSpeedProcessing()
        {
            float currentSpeed = rb2d.velocity.magnitude;
            //SetDebugText(string.Format("Current speed: {0}", currentSpeed));
        }

        private void HandlePlayerLeavingFloor()
        {
            if(TouchingFloorObjects <= 0 && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.Shrinking);
                StopVelocity();
            }
        }

        public bool IsPlayerAtPowerSpeed()
        {
            return rb2d.velocity.magnitude >= PowerSpeed;
        }

        void OnCollisionEnter2D(Collision2D collider)
        {
            int layer = collider.gameObject.layer;
        }
    
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.FloorSlopeDown.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int) SpriteEffects.DownSlope);
                TouchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeUp.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.UpSlope);
                TouchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeRight.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.RightSlope);
                TouchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeLeft.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.LeftSlope);
                TouchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.Floor.ToString()))
            {
                TouchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.PickupSpeed.ToString()) && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                other.gameObject.SetActive(false);
                IncrementTempPowerUpCount(PowerupTypes.Speed);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.FloorSlopeDown.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int) SpriteEffects.DownSlope);
                DownwardForce = 0;
                TouchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeUp.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.UpSlope);
                UpwardForce = 0;
                TouchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeRight.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.RightSlope);
                RightForce = 0;
                TouchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeLeft.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.LeftSlope);
                UpwardForce = 0;
                TouchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.Floor.ToString()))
            {
                TouchingFloorObjects--;
            }
        }

        private void SetDebugText(string text)
        {
            debugText.text = text;
        }
    }
}
