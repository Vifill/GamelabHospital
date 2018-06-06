using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTurnOffCollectable : MonoBehaviour {

    private Collectable collectable;
	void Start () {
        collectable = GetComponent<Collectable>();
	}
	
	// Update is called once per frame
	void Update () {
        if (collectable != null)
        {
            if (collectable.CollectableModel.IsFound)
            {
                Destroy(GetComponent<Animator>());
            }
        }
	}
}
