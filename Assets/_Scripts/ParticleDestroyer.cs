using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour 
{
    public float LifeTime;

	private void Start() 
	{
        Destroy(gameObject, LifeTime);
	}

}
