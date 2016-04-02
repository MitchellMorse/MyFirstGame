using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : GenericSprite
{
    public float speed;

    private Rigidbody2D rb2d;
    private int count;

    protected override void Start()
    {
        base.Start();

        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float moveHorizontal = Input.GetAxis("Horizontal") + RightForce - LeftForce;
        float moveVertical = Input.GetAxis("Vertical") - DownwardForce + UpwardForce;

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
            this.CurrentState = SpriteState.Shrinking;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
        }
    }
}
