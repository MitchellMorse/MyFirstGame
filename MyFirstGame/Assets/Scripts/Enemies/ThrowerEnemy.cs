using UnityEngine;
using System.Collections;
using Assets.Scripts.Abstract;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class ThrowerEnemy : NonPlayerMoving
{
    public MovementTypes _movementType;
    public int _maxTimeCount;

    private int _rotationTimeCount;
    private int _rotationTimeMax;
    private bool _objectGrabbed;
    private GenericSprite _grabbedObject;

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
        _objectGrabbed = false;
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
        else if (collider.gameObject.CompareTag(Tags.Player.ToString()) || collider.gameObject.CompareTag(Tags.DamagingEnemy.ToString()))
        {
            if (_objectGrabbed) return;

            HandleGenericSpriteCollision(collider.gameObject.GetComponent<GenericSprite>());
            this.StopVelocity();
        }
    }

    protected override void HandlePlayerCollision(PlayerController player)
    {
        HandleGenericSpriteCollision(player);
    }

    private void HandleGenericSpriteCollision(GenericSprite sprite)
    {
        sprite.StopVelocity();
        sprite.AddStatusEffect(SpriteEffects.ControlledByOtherObject);

        _rotationTimeMax = Random.Range(100, 400);
        _objectGrabbed = true;
        _grabbedObject = sprite;
    }

    private void HandleSpinning()
    {
        if (!_objectGrabbed) return;

        _rotationTimeCount++;
        transform.Rotate(new Vector3(0, 0, 45) * (_rotationTimeCount + 1));

        _grabbedObject.transform.RotateAround(this.transform.position, Vector3.forward, _rotationTimeCount + 1);

        if (_rotationTimeCount >= _rotationTimeMax)
        {
            Vector2 dir = transform.position.CalculateOppositeVector(_grabbedObject.transform.position);

            _grabbedObject.moveHorizontal = dir.x;
            _grabbedObject.moveVertical = dir.y;
            _grabbedObject.AddForce(_rotationTimeCount * 10);

            _grabbedObject.RemoveStatusEffect(SpriteEffects.ControlledByOtherObject);

            _rotationTimeCount = 0;
            _objectGrabbed = false;
        }
    }
}
