using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : GenericSprite
{
    public float speed;
    public Text debugText;
    public int fuelCount;
    public int fuelCountDecrementMax;

    private Rigidbody2D rb2d;
    private int TouchingFloorObjects;
    private float CurrentAcceleration;
    private float PowerSpeed = 20f;
    private int FuelCountDecrement;
    
    //this is an experiment
    private float decelTime = 10;
    private float currenDecelTime = 0;
    private Vector3 testSpeed;

    protected override void Start()
    {
        base.Start();

        rb2d = GetComponent<Rigidbody2D>();
        TouchingFloorObjects = 0;
        CurrentAcceleration = 0f;
        FuelCountDecrement = 0;
    }

    protected override void Update()
    {
        base.Update();

        float moveVertical, moveHorizontal;
        CheckForPlayerInput(out moveVertical, out moveHorizontal);

        //SetDebugText(string.Format("Horizontal: {0}.  Vertical: {1}.", moveHorizontal, moveVertical));

        Vector2 movement = new Vector2(moveHorizontal, moveVertical); //- vertical goes down

        if (moveHorizontal != 0f || moveVertical != 0f)
        {
            rb2d.AddForce(movement*speed);
        } 
        else if(transform.position.magnitude > 0  )
        {
            float interpolatingFactor = currenDecelTime/decelTime;
            Vector3 move = Vector3.Slerp(testSpeed, Vector3.zero, interpolatingFactor);

            transform.position -= move;
            currenDecelTime += Time.deltaTime;
        }

        PlayerSpeedProcessing();
        HandlePlayerLeavingFloor();
        UpdateFuelCount();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveVertical"></param>
    /// <param name="moveHorizontal"></param>
    /// <returns>true if any input is found</returns>
    private bool CheckForPlayerInput(out float moveVertical, out float moveHorizontal)
    {
        moveHorizontal = moveVertical = 0f;

        if (fuelCount > 0)
        {
            moveHorizontal = CurrentState.CheckForExistenceOfBit((int) SpriteState.Shrinking)
                ? 0
                : Input.GetAxis("Horizontal") + RightForce - LeftForce;
            moveVertical = CurrentState.CheckForExistenceOfBit((int) SpriteState.Shrinking)
                ? 0
                : Input.GetAxis("Vertical") - DownwardForce + UpwardForce;
        }

        return moveVertical != 0 || moveHorizontal != 0;
    }

    private void UpdateFuelCount()
    {
        float moveVertical, moveHorizontal;
        bool playerMoving = CheckForPlayerInput(out moveVertical, out moveHorizontal);

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
        if(TouchingFloorObjects <= 0)
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.Shrinking);
            StopVelocity();
        }
    }

    public bool IsPlayerAtPowerSpeed()
    {
        return rb2d.velocity.magnitude >= PowerSpeed;
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

    private void SetDebugText(string text)
    {
        debugText.text = text;
    }
}
