using Assets._Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    public AudioClip DashSound;
    public GameObject DashParticles;
    public GameObject MovementParticle;
    private Animator Animator;
    private AudioSource AudioSource;
    private ParticleSystem.EmissionModule EmissionModule;
    public float Speed = 10;
    public float RotSpeed = 1;
    public float YOffset = 1f;
    public bool CanMove;
    public bool CanDash;
    public bool IsDashing;
    public float WindUpTime = 5;

    private Vector3 Direction;


    // Use this for initialization
    private void Start ()
    {
        CanDash = true;
        CanMove = true;
        Animator = GetComponentInChildren<Animator>();
        AudioSource = GetComponent<AudioSource>();

        if (MovementParticle != null)
        {
            GameObject tempParticle = ((GameObject)Instantiate(MovementParticle, new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z), transform.rotation, transform));
            EmissionModule = tempParticle.GetComponentInChildren<ParticleSystem>().emission;
            EmissionModule.enabled = false;
        }
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (CanDash && CanMove && Input.GetButtonDown("Fire3"))
        {
            Dash();
        }
        if (CanMove)
        {
            WalkInput();
        }
        else
        {
            if (Animator.GetBool(Constants.AnimationParameters.CharacterIsWalking))
            {
                Animator.SetBool(Constants.AnimationParameters.CharacterIsWalking, false);
                EmissionModule.enabled = false;
            }
        }

        if (IsDashing && !EmissionModule.enabled)
        {
            EmissionModule.enabled = true;
        }
    }

    protected virtual void WalkInput()
    {
        if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            Direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (Direction.magnitude > 1)
            {
                Direction.Normalize();
            }
            if (!IsDashing)
            {
                transform.GetComponent<CharacterController>().Move(Direction * Time.deltaTime * Speed);
                transform.position = new Vector3(transform.position.x, YOffset, transform.position.z);
            }
           

            //if (Direction.magnitude >= 0.1)
            //{
            //    var lookRot = Quaternion.LookRotation(Direction);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, RotSpeed * Time.deltaTime);

            //    if (!Animator.GetBool(Constants.AnimationParameters.CharacterIsWalking))
            //    {
            //        StartWalkingAnimation();
            //    }
            //}

            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            {
                var lookRot = Quaternion.LookRotation(Direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, RotSpeed * Time.deltaTime);

                if (!Animator.GetBool(Constants.AnimationParameters.CharacterIsWalking))
                {
                    StartWalkingAnimation();
                }
            }
        }
        else
        {
            if (Animator.GetBool(Constants.AnimationParameters.CharacterIsWalking))
            {
                StopWalkingAnimation();
            }
        }
    }

    private void Dash()
    {

        IsDashing = true;
        //CanMove = false;
        CanDash = false;
        CharacterController charController = GetComponent<CharacterController>();
        StartCoroutine(DashTo(charController));
    }

    private IEnumerator DashTo(CharacterController pCharController)
    {
        GameObject particlesystem = null;
        if (DashSound != null)
        {
            AudioSource?.PlayOneShot(DashSound);
        }
        if (DashParticles != null)
        {
           particlesystem = (GameObject)Instantiate(DashParticles, transform.position, transform.rotation, transform);
        }
        float dashTimer = 0;
        float SpeedMultiplier = 9f;
        float DashDuration = .2f;

        while (dashTimer < DashDuration)
        {
           
            float currentSpeedMultiplier = -Mathf.Pow((dashTimer / DashDuration) * (1 - (-1)) + (-1), 4) + 1; // -(ratio * (max+min) + min) + 1 // y = x^4 + 1
            //print(currentSpeedMultiplier);
            //SpeedMultiplier = Mathf.Lerp(SpeedMultiplier, 1, );
            pCharController.Move(transform.forward * Time.deltaTime * (Speed + (currentSpeedMultiplier * SpeedMultiplier)));
            dashTimer += Time.deltaTime;
            yield return null;
        }
        //CanMove = true;
        IsDashing = false;
        if (particlesystem != null)
        {
            Destroy(particlesystem, 3f);
        }

        yield return new WaitForSeconds(.5f);
        CanDash = true;
    }

    private void StartWalkingAnimation()
    {
        Animator.SetBool(Constants.AnimationParameters.CharacterIsWalking, true);
        EmissionModule.enabled = true;
    }

    private void StopWalkingAnimation()
    {
        Animator.SetBool(Constants.AnimationParameters.CharacterIsWalking, false);
        EmissionModule.enabled = false;
    }

    public void StopMovement()
    {
        CanMove = false;
        StopWalkingAnimation();
    }

    public void StartMovement()
    {
        CanMove = true;
    }
}
