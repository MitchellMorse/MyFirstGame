using UnityEngine;
using System.Collections;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class ThrowerEnemy : NonPlayerMoving
{
    public MovementTypes _movementType;
    public int _maxTimeCount;

    private int _rotationTimeCount;
    private int _rotationTimeMax;
    private bool _playerGrabbed;
    private PlayerController playerObject;

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

        _rotationTimeCount = 0;
        _rotationTimeMax = 0;
        _playerGrabbed = false;
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
	    base.Update();

        HandleSpinning();
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
            this.StopVelocity();
        }
    }

    private void HandlePlayerCollision(PlayerController player)
    {
        player.StopVelocity();
        player.AddStatusEffect(SpriteEffects.ControlledByOtherObject);

        _rotationTimeMax = Random.Range(100, 400);
        _playerGrabbed = true;
        playerObject = player;
    }

    private void HandleSpinning()
    {
        if (!_playerGrabbed) return;

        _rotationTimeCount++;
        transform.Rotate(new Vector3(0, 0, 45) * (_rotationTimeCount + 1));

        playerObject.transform.RotateAround(this.transform.position, Vector3.forward, _rotationTimeCount + 1);

        if (_rotationTimeCount >= _rotationTimeMax)
        {
            Vector2 dir = transform.position.CalculateOppositeVector(playerObject.transform.position);

            playerObject.moveHorizontal = dir.x;
            playerObject.moveVertical = dir.y;
            playerObject.AddForce(_rotationTimeCount * 10);

            playerObject.RemoveStatusEffect(SpriteEffects.ControlledByOtherObject);

            _rotationTimeCount = 0;
            _playerGrabbed = false;
        }
    }
}
