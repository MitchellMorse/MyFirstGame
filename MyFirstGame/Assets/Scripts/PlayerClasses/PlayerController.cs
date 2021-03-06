﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Assets.Scripts.Abstract;
using Assets.Scripts.Utilities;
using Models;
using UnityEngine;
using UnityEngine.Experimental.Networking;
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

        public Text debugText;

        [HideInInspector]
        public int CurrentPlayerEffects;

        private List<Sprite> _sprites;
        private float _powerSpeed = 20f;
        private int _fuelCountDecrement;
        private float _currenDecelTime = 0;
        private float _maxJumpScale;
        private int _invincibleCount;
        private int _maxInvincible;

        private float _mouseXMovement;
        private float _mouseYMovement;

        private const float _mouseModifier = .03f;

        protected override void Start()
        {
            base.Start();
        
            CurrentPlayerEffects = (int)PlayerEffects.Normal;
            _fuelCountDecrement = 0;
            InitializePowerupList(new PlayerStats());
            SetInitialPowerupSettings();
            moveVertical = moveHorizontal = 0;
            _maxJumpScale = transform.localScale.x*2;
            LoadSprites();
            MaxHealth = 4;
            CurrentHealth = 4;

            _invincibleCount = 0;
            _maxInvincible = 200;


            SetPowerupHudImages();
            
            StartCoroutine(GetTestStuff());
        }

        IEnumerator GetTestStuff()
        {
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:64279/test/get?id=1");
            yield return request.Send();

            if (request.isError)
            {
                //do something...
            }
            else
            {
                string results = request.downloadHandler.text;
                TestStuff test = JsonUtility.FromJson<TestStuff>(results);
                debugText.text = test.TestData;
            }
        } 

        private void LoadSprites()
        {
            _sprites = new List<Sprite>();
            _sprites.Add(Resources.Load<Sprite>("Sprites/Pickups/JumpPickup"));
            _sprites.Add(Resources.Load<Sprite>("Sprites/Pickups/SpeedPickup"));

            _sprites.Add(Resources.Load<Sprite>("Sprites/Player/UFO"));
            _sprites.Add(Resources.Load<Sprite>("Sprites/Player/Player1Damage"));
            _sprites.Add(Resources.Load<Sprite>("Sprites/Player/Player2Damage"));
            _sprites.Add(Resources.Load<Sprite>("Sprites/Player/Player3Damage"));
        }

        protected override void Update()
        {
            base.Update();
        
            CheckForPlayerInput();

            AddForce();
            HandleJumpOngoing();
            UpdateHud();

            moveVertical = moveHorizontal = 0;

            if (CurrentState.CheckForExistenceOfBit((int) SpriteEffects.Invincible))
            {
                _invincibleCount++;

                if (_invincibleCount >= _maxInvincible)
                {
                    CurrentState = CurrentState.RemoveBitFromInt((int) SpriteEffects.Invincible);
                    _invincibleCount = 0;
                }
            }
        }

        public override void DamageObject(int damageAmount = 1)
        {
            base.DamageObject(damageAmount);

            SpriteRenderer playerImage = GetComponent<SpriteRenderer>();
            CurrentState = CurrentState.AddBitToInt((int) SpriteEffects.Invincible);

            switch (CurrentHealth)
            {
                case 3:
                    playerImage.sprite = _sprites.Single(s => s.name == "Player1Damage");
                    break;
                case 2:
                    playerImage.sprite = _sprites.Single(s => s.name == "Player2Damage");
                    break;
                case 1:
                    playerImage.sprite = _sprites.Single(s => s.name == "Player3Damage");
                    break;
                default:
                    CurrentState = CurrentState.AddBitToInt((int) SpriteEffects.Dead);
                    HandleObjectDestruction();
                    break;
            }
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

        private bool CanPlayerMove()
        {
            return !CurrentState.CheckForExistenceOfBit((int) SpriteEffects.Shrinking) &&
                   !CurrentState.CheckForExistenceOfBit((int) SpriteEffects.Dead) &&
                   !CurrentState.CheckForExistenceOfBit((int) SpriteEffects.ControlledByOtherObject);
        }

        private void CheckForMovementInput(ref float moveVertical, ref float moveHorizontal)
        {
            HandleMouseInput();

            //HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            float xAxis = Input.GetAxis("Horizontal");
            moveHorizontal = !CanPlayerMove()
                ? 0
                : xAxis + RightForce - LeftForce;

            float yAxis = Input.GetAxis("Vertical");
            moveVertical = !CanPlayerMove()
                ? 0
                : yAxis - DownwardForce + UpwardForce;

            debugText.text = string.Format("x: {0}; y: {1}", xAxis, yAxis);
        }

        private void HandleMouseInput()
        {
            bool mouseClicked = Input.GetMouseButton(0);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 movementVector = transform.position.CalculateVectorTowards(mousePosition).normalized;

            if (mouseClicked)
            {
                moveHorizontal = !CanPlayerMove()
                    ? 0
                    : movementVector.x + RightForce - LeftForce;

                moveVertical = !CanPlayerMove()
                    ? 0
                    : movementVector.y - DownwardForce + UpwardForce;
            }
            else
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb.velocity.magnitude > 0)
                {
                    rb.velocity = rb.velocity*0.99f;
                }
            }
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

        protected override void OnCollisionEnter2D(Collision2D collider)
        {
            base.OnCollisionEnter2D(collider);
        }
    
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (other.gameObject.CompareTag(Tags.PickupSpeed.ToString()) && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                other.gameObject.SetActive(false);
                IncrementTempPowerUpCount(PowerupTypes.Speed);
            }
            else if (other.gameObject.CompareTag(Tags.LevelGoal.ToString()))
            {
                CurrentPlayerEffects = CurrentPlayerEffects.AddBitToInt((int) PlayerEffects.EndOfLevelReached);
            }
        }

        private void SetDebugText(string text)
        {
            fuelText.text = text;
        }
    }
}
