using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Footsteps : MonoBehaviour 
{
    public List<AudioClip> FootstepsSounds;
    public float TimeBetweenSteps;
    public AudioSource ASource;

    private float Timer = 0;
    private Vector3 Movement;

    private void PlayFootstepSound()
    {
        var rng = Random.Range(0, 4);
        ASource.PlayOneShot(FootstepsSounds[rng]);
    }

	// Update is called once per frame
	private void Update() 
	{
        Timer += Time.deltaTime;
        Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Timer >= TimeBetweenSteps && Movement.magnitude != 0)
        {
            Timer = 0;
            PlayFootstepSound();
        }
	}
}
