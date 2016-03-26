using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;

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
        transform.position = player.transform.position + offset;
    }
}
