using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapping : MonoBehaviour {

	private ParticleSystem ParticleSystem;
    private ParticleSystem.EmissionModule EmissionModule;
    private ParticleSystemRenderer ParticleSystemRenderer;
    private float Rate = 0;
    private float Timer = 0;
    private int Index = 0;
    public Material[] Materials;

	void Start () {
        ParticleSystemRenderer = GetComponent<ParticleSystemRenderer>();
        ParticleSystem = GetComponent<ParticleSystem>();
        EmissionModule = ParticleSystem.emission;
        Rate = 1/EmissionModule.rateOverTime.constant;
    }
	
	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;
        if (Timer >= Rate)
        {

        }
	}

    void SwapMaterial()
    {
        ParticleSystemRenderer.material = Materials[Index];
        Index++;
    }
}
