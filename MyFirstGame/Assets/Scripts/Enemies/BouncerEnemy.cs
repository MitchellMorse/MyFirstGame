using UnityEngine;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class BouncerEnemy : NonPlayerMoving
{
    public MovementTypes _movementType;
    public int _maxTimeCount;

    // Use this for initialization
    protected override int MaxTimeCount
    {
        get { return _maxTimeCount; }
    }

    protected override MovementTypes MovementType
    {
        get { return _movementType; }
    }

    protected override TravelTypes TravelType
    {
        get { return TravelTypes.Ground; }
    }

    protected override void Start()
    { 
        base.Start();
        
        _maxGrowthScale = transform.localScale.x * 1.3f;

        if (_maxTimeCount <= 0) _maxTimeCount = 200;
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

    protected override void HandlePlayerCollision(PlayerController player)
    {
        base.HandlePlayerCollision(player);

        player.StopVelocity();
        
        Vector2 dir = transform.position.CalculateVectorTowards(player.transform.position);

        player.moveHorizontal = dir.x;
        player.moveVertical = dir.y;
        player.AddForce(1000);
    }

    
}
