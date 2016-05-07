using UnityEngine;
using System.Collections;
using Assets.Scripts.Abstract;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class SuctionEnemy : NonPlayerMoving
{
    public MovementTypes _movementType;
    public int _maxTimeCount;

    public float SuctionStrength;
    public float SuctionRadius;
    public int SuctionPulseRate;
    public bool Sucks;

    private int _suctionTimeCount;

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

    protected override void Start ()
    {
	    base.Start();

        _suctionTimeCount = 0;

        if (SuctionPulseRate <= 0)
        {
            SuctionPulseRate = 100;
        }

        if (SuctionRadius <= 0)
        {
            SuctionRadius = 10;
        }

        if (SuctionStrength <= 0)
        {
            SuctionStrength = 1000;
        }
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
	    base.Update();

        SuctionPulse();
	}

    private void SuctionPulse()
    {
        if (_suctionTimeCount <= SuctionPulseRate)
        {
            _suctionTimeCount++;
            return;
        }

        _suctionTimeCount = 0;

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, SuctionRadius);

        foreach (Collider2D collisionObject in objects)
        {
            GenericSprite sprite = collisionObject.gameObject.GetComponent<GenericSprite>();

            if (sprite == null) continue;
            
            Vector2 vector = Sucks
            ? sprite.transform.position.CalculateVectorTowards(transform.position)
            : transform.position.CalculateVectorTowards(sprite.transform.position);

            float distance = Vector2.Distance(transform.position, sprite.transform.position);
            float force = SuctionStrength/distance;

            sprite.AddForce(force, vector.x, vector.y);
        }
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
        }
        else if (collider.gameObject.CompareTag(Tags.DamagingEnemy.ToString()))
        {
            HandleSpriteCollision(collider.gameObject.GetComponent<GenericSprite>());
        }
    }

    protected override void HandlePlayerCollision(PlayerController player)
    {
        HandleSpriteCollision(player);
    }

    private void HandleSpriteCollision(GenericSprite player)
    {
        player.StopVelocity();

        Vector2 dir = transform.position.CalculateVectorTowards(player.transform.position);

        player.moveHorizontal = dir.x;
        player.moveVertical = dir.y;
        player.AddForce(500);
        player.DamageObject();
    }
}
