using UnityEngine;
using System.Collections;

public class MoveBoard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		var ry = Input.GetAxis("Horizontal") * 2f;
		var rx = Input.GetAxis("Vertical") * 2f;

		//transform.rotation = Quaternion.AngleAxis(r, Vector3.right); 
		transform.Rotate(rx ,ry ,0);

        if (Input.GetKeyDown("r"))
        {
            transform.rotation = Quaternion.identity;
        }
            
    }
}
