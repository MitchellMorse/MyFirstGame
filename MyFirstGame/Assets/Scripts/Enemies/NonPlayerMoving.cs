using UnityEngine;
using System.Collections;
using Assets.Scripts.Abstract;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public abstract class NonPlayerMoving : GenericSprite
{
    protected enum TravelTypes
    {
        Ground,
        Air
    }

    public enum MovementTypes
    {
        Wandering,
        FollowingPlayer,
        Stationary
    }

    private int _timeCount;

    protected float _maxGrowthScale;
    protected abstract int MaxTimeCount { get; }
    protected abstract MovementTypes MovementType { get; }
    protected abstract TravelTypes TravelType { get; }
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

        _timeCount = 0;

        InitializeMovement();
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
	    base.Update();

        AddForce();

        _timeCount++;
        if (_timeCount >= MaxTimeCount)
        {
            UpdateMovement();
            _timeCount = 0;
        }
    }

    /// <summary>
    /// This will completely stop the object and then reinitialize the movement direction
    /// </summary>
    protected void InitializeMovement()
    {
        this.StopVelocity();

        switch (MovementType)
        {
            case MovementTypes.Wandering:
                InitializeWanderingMovement();
                break;
            case MovementTypes.FollowingPlayer:
                InitializePlayerTargettingMovement();
                break;
            case MovementTypes.Stationary:
                InitializeStationaryMovement();
                break;
        }
    }

    /// <summary>
    /// This will not stop the object, and will update the movement direction
    /// </summary>
    protected void UpdateMovement()
    {
        switch (MovementType)
        {
            case MovementTypes.Wandering:
                InitializeWanderingMovement();
                break;
            case MovementTypes.FollowingPlayer:
                InitializePlayerTargettingMovement();
                break;
            case MovementTypes.Stationary:
                InitializeStationaryMovement();
                break;
        }
    }

    #region Wandering

    private void InitializeWanderingMovement()
    {
        if (CurrentlyCollidingObjects.Count > 0)
        {
            var direction = transform.position.CalculateVectorTowards(CurrentlyCollidingObjects[0].transform.position);

            moveHorizontal = direction.x;
            moveVertical = direction.y;
        }
        else
        {
            moveVertical = Random.Range(-1f, 1f);
            moveHorizontal = Random.Range(-1f, 1f);
        }
    }

    #endregion

    #region Following player

    private void InitializePlayerTargettingMovement()
    {
        bool playerIsInFollowableState = true;
        PlayerController player = null;

        GameObject playerObject = GameObject.Find("Player");

        if (playerObject == null)
        {
            playerIsInFollowableState = false;
        }
        else
        {
            player = playerObject.GetComponent<PlayerController>();

            if(player.CurrentState.CheckForExistenceOfBit((int)SpriteEffects.ControlledByOtherObject))
            {
                playerIsInFollowableState = false;
            }
        }

        if (playerIsInFollowableState)
        {
            Vector2 vectorToPlayer = transform.position.CalculateVectorTowards(player.transform.position);

            moveHorizontal = vectorToPlayer.x;
            moveVertical = vectorToPlayer.y;
        }
        else
        {
            InitializeWanderingMovement();
        }
    }
    #endregion

    #region Stationary

    private void InitializeStationaryMovement()
    {
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);

        moveHorizontal = 0;
        moveVertical = 0;
    }
    #endregion

    protected void HandleGrowAnimationOngoing()
    {
        if (!CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Growing)) return;

        bool finished = GrowShrinkAnimation(_maxGrowthScale, OriginalScale, .05f);

        if (finished)
        {
            CurrentState = CurrentState.RemoveBitFromInt((int) SpriteEffects.Growing);
        }
    }

    protected virtual void HandlePlayerCollision(PlayerController player)
    {
        this.StopVelocity();

        Vector2 vectorAwayFromPlayer = player.transform.position.CalculateVectorTowards(transform.position);

        moveHorizontal = vectorAwayFromPlayer.x;
        moveVertical = vectorAwayFromPlayer.y;

        _timeCount = MaxTimeCount - 50;
        AddForce(500);
    }
}
