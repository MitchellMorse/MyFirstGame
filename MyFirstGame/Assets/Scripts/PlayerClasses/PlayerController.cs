using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public partial class PlayerController : GenericSprite
{
    public float speed;
    public Text debugText;
    public int fuelCount;
    public int fuelCountDecrementMax;
    public Text speedCountText;

    [HideInInspector]
    public PlayerStats _playerStats;

    private Rigidbody2D rb2d;
    private int TouchingFloorObjects;
    private float CurrentAcceleration;
    private float PowerSpeed = 20f;
    private int FuelCountDecrement;
    private float decelTime = 10;
    private float currenDecelTime = 0;
    private Vector3 testSpeed;
    private float moveVertical;
    private float moveHorizontal;

    protected override void Start()
    {
        base.Start();

        rb2d = GetComponent<Rigidbody2D>();
        TouchingFloorObjects = 0;
        CurrentAcceleration = 0f;
        FuelCountDecrement = 0;
        _playerStats = new PlayerStats();
        moveVertical = moveHorizontal = 0;
    }

    protected override void Update()
    {
        base.Update();
        
        CheckForPlayerInput();

        Vector2 movement = new Vector2(moveHorizontal, moveVertical); //- vertical goes down

        if (moveHorizontal != 0f || moveVertical != 0f)
        {
            rb2d.AddForce(movement*speed);
        }

        PlayerSpeedProcessing();
        HandlePlayerLeavingFloor();
        UpdateHud();

        moveVertical = moveHorizontal = 0;
    }

    private void UpdateHud()
    {
        UpdateFuelCount(moveVertical, moveHorizontal);

        speedCountText.text = string.Format("Speed Powerup Count: {0}",
            _playerStats.PermanentSpeedCount + _playerStats.TemporarySpeedCount);
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

        if (fuelCount > 0)
        {
            CheckForMovementInput(ref moveVertical, ref moveHorizontal);
        }

        CheckForPowerupInput();

        return moveVertical != 0 || moveHorizontal != 0;
    }

    private void CheckForMovementInput(ref float moveVertical, ref float moveHorizontal)
    {
        moveHorizontal = CurrentState.CheckForExistenceOfBit((int) SpriteState.Shrinking)
            ? 0
            : Input.GetAxis("Horizontal") + RightForce - LeftForce;
        moveVertical = CurrentState.CheckForExistenceOfBit((int) SpriteState.Shrinking)
            ? 0
            : Input.GetAxis("Vertical") - DownwardForce + UpwardForce;
    }

    private void UpdateFuelCount(float moveVertical, float moveHorizontal)
    {
        bool playerMoving = moveVertical != 0 || moveHorizontal != 0;

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
        if (other.gameObject.CompareTag(Tags.FloorSlopeDown.ToString()))
        {
            CurrentState = CurrentState.AddBitToInt((int) SpriteState.DownSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag(Tags.FloorSlopeUp.ToString()))
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.UpSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag(Tags.FloorSlopeRight.ToString()))
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.RightSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag(Tags.FloorSlopeLeft.ToString()))
        {
            CurrentState = CurrentState.AddBitToInt((int)SpriteState.LeftSlope);
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag(Tags.Floor.ToString()))
        {
            TouchingFloorObjects++;
        }
        else if (other.gameObject.CompareTag(Tags.PickupSpeed.ToString()))
        {
            other.gameObject.SetActive(false);
            _playerStats.TemporarySpeedCount += 1;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.FloorSlopeDown.ToString()))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int) SpriteState.DownSlope);
            DownwardForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag(Tags.FloorSlopeUp.ToString()))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int)SpriteState.UpSlope);
            UpwardForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag(Tags.FloorSlopeRight.ToString()))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int)SpriteState.RightSlope);
            RightForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag(Tags.FloorSlopeLeft.ToString()))
        {
            CurrentState = CurrentState.RemoveBitFromInt((int)SpriteState.LeftSlope);
            UpwardForce = 0;
            TouchingFloorObjects--;
        }
        else if (other.gameObject.CompareTag(Tags.Floor.ToString()))
        {
            TouchingFloorObjects--;
        }
    }

    private void SetDebugText(string text)
    {
        debugText.text = text;
    }
}
