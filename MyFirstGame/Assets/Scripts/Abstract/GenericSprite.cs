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
        CurrentState |= (int)SpriteState.Normal;
    }

    protected virtual void FixedUpdate()
    {
        HandleState();
    }

    protected virtual void HandleState()
    {
        if ((CurrentState & (int)SpriteState.DownSlope) != 0)
        {
            if (DownwardForce < MaxPullForce)
            {
                DownwardForce += AmountOfForceToAddPerUpdate;
            }
        }

        if ((CurrentState & (int)SpriteState.UpSlope) != 0)
        {
            if (UpwardForce < MaxPullForce)
            {
                UpwardForce += AmountOfForceToAddPerUpdate;
            }
        }

        if ((CurrentState & (int)SpriteState.RightSlope) != 0)
        {
            if (RightForce < MaxPullForce)
            {
                RightForce += AmountOfForceToAddPerUpdate;
            }
        }

        if ((CurrentState & (int)SpriteState.LeftSlope) != 0)
        {
            if (LeftForce < MaxPullForce)
            {
                LeftForce += AmountOfForceToAddPerUpdate;
            }
        }

        if ((CurrentState & (int)SpriteState.Shrinking) != 0)
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
