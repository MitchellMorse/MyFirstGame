﻿using UnityEngine;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class BouncerEnemy : NonPlayerMoving
{
    private float _maxGrowthScale;

    // Use this for initialization
    protected override int MaxTimeCount
    {
        get { return 200; }
    }

    protected override MovementTypes MovementType
    {
        get { return MovementTypes.Wandering; }
    }

    protected override TravelTypes TravelType
    {
        get { return TravelTypes.Ground; }
    }

    protected override void Start()
    { 
        base.Start();
        
        _maxGrowthScale = transform.localScale.x * 1.3f;
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
	    base.Update();

	    HandleGrowAnimationOngoing();
    }
    

    protected override void OnCollisionEnter2D(Collision2D collider)
    {
        base.OnCollisionEnter2D(collider);

        if (collider.gameObject.CompareTag(Tags.Wall.ToString()))
        {
            InitializeMovement();
        }
        else if (collider.gameObject.CompareTag(Tags.Player.ToString()))
        {
            HandlePlayerCollision(collider.gameObject.GetComponent<PlayerController>());

            //want to make a quick growth animation where the thing grows and shrinks upon hitting the player
            CurrentState = CurrentState.AddBitToInt((int)SpriteEffects.Growing);
        }
    }

    private void HandlePlayerCollision(PlayerController player)
    {
        player.StopVelocity();
        
        Vector2 dir = transform.position.CalculateOppositeVector(player.transform.position);

        player.moveHorizontal = dir.x;
        player.moveVertical = dir.y;
        player.AddForce(1000);
    }

    private void HandleGrowAnimationOngoing()
    {
        if (!CurrentState.CheckForExistenceOfBit((int)SpriteEffects.Growing)) return;

        bool finished = GrowShrinkAnimation(_maxGrowthScale, OriginalScale, .05f);

        if (finished)
        {
            CurrentState = CurrentState.RemoveBitFromInt((int) SpriteEffects.Growing);
        }
    }
}