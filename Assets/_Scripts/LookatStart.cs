using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatStart : MonoBehaviour {

	void Start ()
	{
	    Vector3 dir = (Camera.main.transform.position - transform.position);
	    dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
	}

}
