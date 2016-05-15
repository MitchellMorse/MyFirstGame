using UnityEngine;
using System.Collections;
using Assets.Scripts.PlayerClasses;
using Assets.Scripts.Utilities;

public class DamagingWall : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player.ToString()))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.StopVelocity();

            Vector2 dir = transform.position.CalculateVectorTowards(player.transform.position);

            player.moveHorizontal = dir.x;
            player.moveVertical = dir.y;
            player.AddForce(500);
            player.DamageObject();
        }
    }
}
