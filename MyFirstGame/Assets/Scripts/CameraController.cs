using Assets.Scripts.PlayerClasses;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {

        public PlayerController player;

        private Vector3 offset;

        // Use this for initialization
        void Start()
        {
            //transform attached to camera since that is what the script is attached to
            offset = transform.position - player.transform.position;
        }

        // Update is called once per frame
        // LateUpdate runs after update
        void LateUpdate()
        {
            transform.position = CalculateCameraPosition();
        }

        private Vector3 CalculateCameraPosition()
        {
            Vector3 retVal;
            if (!player.IsPlayerAtPowerSpeed())
            {
                retVal = player.transform.position + offset;
            }
            else
            {
                float xOffset = Random.Range(0f, .1f);
                float yOffset = Random.Range(0f, .1f);
                retVal = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset) + offset;
            }

            return retVal;
        }
    }
}
