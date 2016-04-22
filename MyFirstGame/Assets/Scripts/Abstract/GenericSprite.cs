using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Abstract
{
    public abstract class GenericSprite : MonoBehaviour
    {
        protected int CurrentState;
        public float AmountToScaleBy = .1f;

        protected float DownwardForce = 0;
        protected float UpwardForce = 0;
        protected float RightForce = 0;
        protected float LeftForce = 0;
        protected Rigidbody2D rb2d;

        private float MaxPullForce = .5f;
        private float AmountOfForceToAddPerUpdate = .05f;
        protected float OriginalScale;

        protected virtual void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            CurrentState.AddBitToInt((int) SpriteEffects.Normal);
            OriginalScale = transform.localScale.x;
        }
        

        protected virtual void Update()
        {
            HandleState();
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
                HandleShrinking();
            }
        }

        protected virtual void HandleShrinking()
        {
            if (transform.localScale.x > 0)
            {
                float newXValue = transform.localScale.x - AmountToScaleBy;
                float newYValue = transform.localScale.y - AmountToScaleBy;
                transform.localScale = new Vector3(newXValue, newYValue, 1);
            }

            if (transform.localScale.x <= 0)
            {
                DestroyObject(this);
            }
        }

        protected virtual float HandleGrowing()
        {
            float newXValue = transform.localScale.x + AmountToScaleBy;
            float newYValue = transform.localScale.y + AmountToScaleBy;
            transform.localScale = new Vector3(newXValue, newYValue, 1);

            return transform.localScale.x;
        }

        protected virtual void StopVelocity()
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
    }
}
