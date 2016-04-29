using UnityEngine;
using System.Collections;
using Assets.Scripts.Abstract;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class BouncerEnemy : NonPlayerMoving
{

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
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
	    base.Update();
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
            //PlayerController player = collider.gameObject as PlayerController;

            //want to make a quick growth animation where the thing grows and shrinks upon hitting the player
            CurrentState.AddBitToInt((int)SpriteEffects.Growing);
        }
    }
}
