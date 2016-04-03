using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : GenericSprite
{
    public float speed;

    private Rigidbody2D rb2d;
    private int TouchingFloorObjects;

    protected override void Start()
    {
        base.Start();

        rb2d = GetComponent<Rigidbody2D>();
        TouchingFloorObjects = 0;
    }

    protected override void Update()
    {
        base.Update();

        float moveHorizontal = CurrentState.CheckForExistenceOfBit((int)SpriteState.Shrinking) ? 0 : Input.GetAxis("Horizontal") + RightForce - LeftForce;
        float moveVertical = CurrentState.CheckForExistenceOfBit((int)SpriteState.Shrinking) ? 0 : Input.GetAxis("Vertical") - DownwardForce + UpwardForce;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical); //- vertical goes down

        rb2d.AddForce(movement * speed);

        HandlePlayerLeavingFloor();
    }

    private void HandlePlayerLeavingFloor()
    {
        //CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();

        //LayerMask floorLayerMask = LayerMask.NameToLayer("Floor");

        //Collider2D overlapCircle = Physics2D.OverlapCircle(transform.position, circleCollider2D.radius, floorLayerMask);

        //if (overlapCircle == null)
        if(TouchingFloorObjects <= 0)
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.Shrinking);
            StopVelocity();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FloorSlopeDown"))
        {
            CurrentState = CurrentState.AddBitToInt((int) SpriteState.DownSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag("FloorSlopeUp"))
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.UpSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag("FloorSlopeRight"))
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.RightSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag("FloorSlopeLeft"))
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.LeftSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag("Floor"))
        {
            TouchingFloorObjects++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FloorSlopeDown"))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int) SpriteState.DownSlope);
            DownwardForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag("FloorSlopeUp"))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int)SpriteState.UpSlope);
            UpwardForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag("FloorSlopeRight"))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int)SpriteState.RightSlope);
            RightForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag("FloorSlopeLeft"))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int)SpriteState.LeftSlope);
            UpwardForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag("Floor"))
        {
            TouchingFloorObjects--;
        }
    }
}
