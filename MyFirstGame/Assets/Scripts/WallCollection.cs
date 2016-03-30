using UnityEngine;
using System.Collections;

public class WallCollection : MonoBehaviour
{

    public int wallCountVertical;
    public int wallCountHorizontal;
    public int wallVerticalPixels = 424;
    public int wallHorizontalPikels = 424;
    public float unityUnitsPerBox = 4.18f;  //this is used here until I have a better understanding of what this number is...
    public GameObject firstSquare;
    public GameObject lastSquare;

    private BoxCollider2D bc2d;

	// Use this for initialization 
	void Start ()
	{
	    //bc2d = GetComponent<BoxCollider2D>();
     //   Vector2 newOffset = new Vector2(firstSquare.transform.position.x, firstSquare.transform.position.y - 8.25f);
     //   Vector2 newSize = new Vector2(wallCountVertical * unityUnitsPerBox, wallCountHorizontal * unityUnitsPerBox);

	    //bc2d.size = newSize;
        

        //bc2d.offset.Set(firstSquare.transform.position.x, firstSquare.transform.position.y);
	    //bc2d.offset = newOffset;
	    //bc2d.offset.Set(-24.5f, -0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
