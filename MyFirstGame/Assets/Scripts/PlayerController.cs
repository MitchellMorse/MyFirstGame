using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : GenericSprite
{
    public float speed;

    private Rigidbody2D rb2d;

    protected override void Start()
    {
        base.Start();

        rb2d = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float moveHorizontal = (CurrentState & (int)SpriteState.Shrinking) != 0 ? 0 : Input.GetAxis("Horizontal") + RightForce - LeftForce;
        float moveVertical = (CurrentState & (int)SpriteState.Shrinking) != 0 ? 0 : Input.GetAxis("Vertical") - DownwardForce + UpwardForce;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical); //- vertical goes down

        rb2d.AddForce(movement * speed);

        HandlePlayerLeavingFloor();
    }

    private void HandlePlayerLeavingFloor()
    {
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        
        Collider2D overlapCircle = Physics2D.OverlapCircle(transform.position, circleCollider2D.radius, 1);
        if (overlapCircle == null)
        {
            this.CurrentState |= (int)SpriteState.Shrinking;
            StopVelocity();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FloorSlopeDown"))
        {
            CurrentState |= (int)SpriteState.DownSlope;
        }
        else if (other.gameObject.CompareTag("FloorSlopeUp"))
        {
            CurrentState |= (int)SpriteState.UpSlope;
        }
        else if (other.gameObject.CompareTag("FloorSlopeRight"))
        {
            CurrentState |= (int)SpriteState.RightSlope;
        }
        else if (other.gameObject.CompareTag("FloorSlopeLeft"))
        {
            CurrentState |= (int)SpriteState.LeftSlope;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FloorSlopeDown"))
        {
            CurrentState &= (int)~SpriteState.DownSlope;
            DownwardForce = 0;
        }
        else if (other.gameObject.CompareTag("FloorSlopeUp"))
        {
            CurrentState &= (int)~SpriteState.UpSlope;
            UpwardForce = 0;
        }
        else if (other.gameObject.CompareTag("FloorSlopeRight"))
        {
            CurrentState &= (int)~SpriteState.RightSlope;
            RightForce = 0;
        }
        else if (other.gameObject.CompareTag("FloorSlopeLeft"))
        {
            CurrentState &= (int)~SpriteState.LeftSlope;
            UpwardForce = 0;
        }
    }
}
