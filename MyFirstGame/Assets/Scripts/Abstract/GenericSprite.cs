using System;
using UnityEngine;
using System.Collections;

public abstract class GenericSprite : MonoBehaviour
{
    protected int CurrentState;
    public float AmountToScaleBy = .1f;

    protected float DownwardForce = 0;
    protected float UpwardForce = 0;
    protected float RightForce = 0;
    protected float LeftForce = 0;

    private float MaxPullForce = .5f;
    private float AmountOfForceToAddPerUpdate = .05f;

    protected virtual void Start()
    {
        CurrentState.AddBitToInt((int) SpriteState.Normal);
    }

    protected virtual void Update()
    {
        HandleState();
    }

    protected virtual void HandleState()
    {
        if (CurrentState.CheckForExistenceOfBit((int)SpriteState.DownSlope))
        {
            if (DownwardForce < MaxPullForce)
            {
                DownwardForce += AmountOfForceToAddPerUpdate;
            }
        }

        if (CurrentState.CheckForExistenceOfBit((int)SpriteState.UpSlope))
        {
            if (UpwardForce < MaxPullForce)
            {
                UpwardForce += AmountOfForceToAddPerUpdate;
            }
        }
        
        if(CurrentState.CheckForExistenceOfBit((int)SpriteState.RightSlope))
        {
            if (RightForce < MaxPullForce)
            {
                RightForce += AmountOfForceToAddPerUpdate;
            }
        }

        if (CurrentState.CheckForExistenceOfBit((int)SpriteState.LeftSlope))
        {
            if (LeftForce < MaxPullForce)
            {
                LeftForce += AmountOfForceToAddPerUpdate;
            }
        }

        if (CurrentState.CheckForExistenceOfBit((int)SpriteState.Shrinking))
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

    protected virtual void StopVelocity()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.zero;
    }
}
