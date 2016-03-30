using UnityEngine;
using System.Collections;

public abstract class GenericSprite : MonoBehaviour
{
    public enum SpriteState
    {
        Normal,
        Shrinking,
        Growing
    }

    public SpriteState CurrentState;
    public float AmountToScaleBy = .1f;

    protected virtual void Start()
    {
        CurrentState = SpriteState.Normal;
    }

    protected virtual void FixedUpdate()
    {
        HandleState();
    }

    protected virtual void HandleState()
    {
        switch (CurrentState)
        {
            case SpriteState.Normal:
                break;
            case SpriteState.Shrinking:
                HandleShrinking();
                break;
            case SpriteState.Growing:
                break;
        }
    }

    protected virtual void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            CurrentState = SpriteState.Shrinking;
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
}
