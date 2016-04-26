using UnityEngine;
using System.Collections;
using Assets.Scripts.Abstract;
using Assets.Scripts.Utilities;

public class WanderingEnemy : GenericSprite
{
    private int _timeCount;
    private int _maxTimeCount = 500;

    // Use this for initialization
    protected override void Start()
    { 
        base.Start();

        _timeCount = 0;

        RandomizeMovementDirection();
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
	    base.Update();

        AddForce();

	    _timeCount++;
	    if (_timeCount >= _maxTimeCount)
	    {
	        RandomizeMovementDirection();
	        _maxTimeCount = 0;
	    }
    }

    private void RandomizeMovementDirection()
    {
        moveVertical = Random.Range(-1f, 1f);
        moveHorizontal = Random.Range(-1f, 1f);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag(Tags.Wall.ToString()))
        {
            StopVelocity();
            RandomizeMovementDirection();
            _timeCount = 0;
        }
    }
}
