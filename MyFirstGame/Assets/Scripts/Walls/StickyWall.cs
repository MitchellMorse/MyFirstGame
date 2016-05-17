using UnityEngine;
using System.Collections;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class StickyWall : MonoBehaviour
{
    private PlayerController _grabbedPlayer;
    private float _pullForce = 10f;
    private float _pullDistance = 10f;

    void Start()
    {
        _grabbedPlayer = null;
    }

    // Update is called once per frame
    void Update ()
    {
        if (_grabbedPlayer != null)
        {
            float distance = Vector2.Distance(transform.position, _grabbedPlayer.transform.position);

            if (distance < _pullDistance)
            {
                PullPlayerTowardsWall();
            }
            else
            {
                _grabbedPlayer = null;
            }
        }
	}

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            _grabbedPlayer = other.gameObject.GetComponent<PlayerController>();            

            _grabbedPlayer.StopVelocity();
            
            PullPlayerTowardsWall();
        }
    }

    private void PullPlayerTowardsWall()
    {
        Vector2 dir = _grabbedPlayer.transform.position.CalculateVectorTowards(transform.position);

        _grabbedPlayer.moveHorizontal = dir.x;
        _grabbedPlayer.moveVertical = dir.y;
        _grabbedPlayer.AddForce(_pullForce);
    }
}
