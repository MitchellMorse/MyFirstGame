using UnityEngine;
using System.Collections;
using Assets.Scripts.Abstract;
using Assets.Scripts.Utilities;

public abstract class NonPlayerMoving : GenericSprite
{
    protected enum TravelTypes
    {
        Ground,
        Air
    }

    protected enum MovementTypes
    {
        Wandering,
        FollowingPlayer
    }

    private int _timeCount;

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
                break;
        }
    }

    #region Wandering

    private void InitializeWanderingMovement()
    {
        if (CurrentlyCollidingObjects.Count > 0)
        {
            var direction = transform.position.CalculateOppositeVector(CurrentlyCollidingObjects[0].transform.position);

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
}
