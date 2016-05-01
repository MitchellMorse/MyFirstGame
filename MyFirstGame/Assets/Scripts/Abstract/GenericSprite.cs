using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Abstract
{
    public abstract class GenericSprite : MonoBehaviour
    {
        protected int CurrentState;
        public float AmountToScaleBy = .1f;
        public float speed;
        [HideInInspector]
        public float moveVertical;
        [HideInInspector]
        public float moveHorizontal;

        protected float DownwardForce = 0;
        protected float UpwardForce = 0;
        protected float RightForce = 0;
        protected float LeftForce = 0;
        protected Rigidbody2D rb2d;

        private float MaxPullForce = .5f;
        private float AmountOfForceToAddPerUpdate = .05f;
        protected float OriginalScale;
        protected int _touchingFloorObjects;

        protected List<GameObject> CurrentlyCollidingObjects;

        protected virtual void Start()
        {
            _touchingFloorObjects = 0;
            rb2d = GetComponent<Rigidbody2D>();
            CurrentState.AddBitToInt((int) SpriteEffects.Normal);
            OriginalScale = transform.localScale.x;
            CurrentlyCollidingObjects = new List<GameObject>();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collider)
        {
            CurrentlyCollidingObjects.Add(collider.gameObject);
        }

        protected virtual void OnCollisionExit2D(Collider2D collider)
        {
            CurrentlyCollidingObjects.Remove(collider.gameObject);
        }


        protected virtual void Update()
        {
            HandleState();

            HandleObjectLeavingFloor();
        }

        protected virtual void HandleState()
        {
            if (CurrentState.CheckForExistenceOfBit((int)SpriteEffects.DownSlope) && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                if (DownwardForce < MaxPullForce)
                {
                    DownwardForce += AmountOfForceToAddPerUpdate;
                }
            }

            if (CurrentState.CheckForExistenceOfBit((int)SpriteEffects.UpSlope) && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                if (UpwardForce < MaxPullForce)
                {
                    UpwardForce += AmountOfForceToAddPerUpdate;
                }
            }
        
            if(CurrentState.CheckForExistenceOfBit((int)SpriteEffects.RightSlope) && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                if (RightForce < MaxPullForce)
                {
                    RightForce += AmountOfForceToAddPerUpdate;
                }
            }

            if (CurrentState.CheckForExistenceOfBit((int)SpriteEffects.LeftSlope) && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                if (LeftForce < MaxPullForce)
                {
                    LeftForce += AmountOfForceToAddPerUpdate;
                }
            }

            if (CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Shrinking))
            {
                HandleShrinking(AmountToScaleBy);
            }
        }

        #region Shrinking and growing
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxXScale"></param>
        /// <param name="originalScale"></param>
        /// <param name="amountToScaleBy"></param>
        /// <returns>true if animation is finished</returns>
        protected virtual bool GrowShrinkAnimation(float maxXScale, float originalScale, float amountToScaleBy)
        {
            bool finished = false;
            if (transform.localScale.x >= maxXScale)
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.MaxHeightReached);
            }

            if (!CurrentState.CheckForExistenceOfBit((int)SpriteEffects.MaxHeightReached))
            {
                HandleGrowing(amountToScaleBy);
            }
            else
            {
                if (transform.localScale.x >= OriginalScale)
                {
                    HandleShrinking(amountToScaleBy);
                }
                else
                {
                    CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.MaxHeightReached);
                    finished = true;
                }
            }

            return finished;
        }

        protected virtual void HandleShrinking(float amountToScaleBy)
        {
            if (transform.localScale.x > 0)
            {
                float newXValue = transform.localScale.x - amountToScaleBy;
                float newYValue = transform.localScale.y - amountToScaleBy;
                transform.localScale = new Vector3(newXValue, newYValue, 1);
            }

            if (transform.localScale.x <= 0)
            {
                DestroyObject(this);
            }
        }

        protected virtual float HandleGrowing(float amountToScaleBy)
        {
            float newXValue = transform.localScale.x + amountToScaleBy;
            float newYValue = transform.localScale.y + amountToScaleBy;
            transform.localScale = new Vector3(newXValue, newYValue, 1);

            return transform.localScale.x;
        }
        #endregion

        public virtual void StopVelocity()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
        }

        public void SendSpriteInNewDirection(float horizontalMovement, float verticalMovement, float forceMultiplier)
        {
            StopVelocity();

            Vector2 movement = new Vector2(horizontalMovement, verticalMovement);
            rb2d.AddForce(movement * forceMultiplier);
        }

        public void AddForce(float? forceSpeed = null)
        {
            if (forceSpeed == null) forceSpeed = speed;

            Vector2 movement = new Vector2(moveHorizontal, moveVertical); //- vertical goes down

            if (moveHorizontal != 0f || moveVertical != 0f)
            {
                rb2d.AddForce(movement * forceSpeed.Value);
            }
        }

        protected void HandleObjectLeavingFloor()
        {
            if (_touchingFloorObjects <= 0 && !CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Airborne))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.Shrinking);
                StopVelocity();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.FloorSlopeDown.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.DownSlope);
                _touchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeUp.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.UpSlope);
                _touchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeRight.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.RightSlope);
                _touchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeLeft.ToString()))
            {
                CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.LeftSlope);
                _touchingFloorObjects++;
            }
            else if (other.gameObject.CompareTag(Tags.Floor.ToString()))
            {
                _touchingFloorObjects++;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.FloorSlopeDown.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.DownSlope);
                DownwardForce = 0;
                _touchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeUp.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.UpSlope);
                UpwardForce = 0;
                _touchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeRight.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.RightSlope);
                RightForce = 0;
                _touchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.FloorSlopeLeft.ToString()))
            {
                CurrentState = CurrentState.RemoveBitFromInt((int)SpriteEffects.LeftSlope);
                UpwardForce = 0;
                _touchingFloorObjects--;
            }
            else if (other.gameObject.CompareTag(Tags.Floor.ToString()))
            {
                _touchingFloorObjects--;
            }
        }
    }
}
