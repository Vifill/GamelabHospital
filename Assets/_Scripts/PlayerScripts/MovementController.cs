using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
    public GameObject MovementParticle;
    private Animator Animator;
    private ParticleSystem.EmissionModule EmissionModule;
    public float Speed = 10;
    public float RotSpeed = 1;
    public float YOffset = 1f;
    public bool CanMove;
    public float WindUpTime = 5;

    private Vector3 Direction;


    // Use this for initialization
    private void Start ()
    {
        CanMove = true;
        Animator = GetComponentInChildren<Animator>();
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
        if (CanMove && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if (CanMove)
        {
            WalkInput();            
        }
        else
        {
            if (Animator.GetBool("IsWalking"))
            {
                Animator.SetBool("IsWalking", false);
                EmissionModule.enabled = false;
            }
        }
    }

    protected virtual void WalkInput()
    {
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            Direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Debug.Log(Direction);
            if (Direction.magnitude > 1)
            {
                Direction.Normalize();
            }

            transform.GetComponent<CharacterController>().Move(Direction * Time.deltaTime * Speed);
            transform.position = new Vector3(transform.position.x, YOffset, transform.position.z);

            if (Direction.magnitude >= 0.1)
            {
                var lookRot = Quaternion.LookRotation(Direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, RotSpeed * Time.deltaTime);

                if (!Animator.GetBool("IsWalking"))
                {
                    StartWalkingAnimation();
                }
            }
        }
        else
        {
            if (Animator.GetBool("IsWalking"))
            {
                StopWalkingAnimation();
            }
        }
    }

    private void Dash()
    {
        CanMove = false;
        CharacterController charController = GetComponent<CharacterController>();
        StartCoroutine(DashTo(charController));
    }

    private IEnumerator DashTo(CharacterController pCharController)
    {
        float dashTimer = 0;
        float SpeedMultiplier = 9f;
        float DashDuration = .2f;

        while (dashTimer < DashDuration)
        {
           
            float currentSpeedMultiplier = -Mathf.Pow((dashTimer / DashDuration) * (1 - (-1)) + (-1), 4) + 1; // -(ratio * (max+min) + min) + 1 // y = x^4 + 1
            print(currentSpeedMultiplier);
            //SpeedMultiplier = Mathf.Lerp(SpeedMultiplier, 1, );
            pCharController.Move(transform.forward * Time.deltaTime * (Speed + (currentSpeedMultiplier * SpeedMultiplier)));
            dashTimer += Time.deltaTime;
            yield return null;
        }
        CanMove = true;
    }

    private void StartWalkingAnimation()
    {
        Animator.SetBool("IsWalking", true);
        EmissionModule.enabled = true;
    }

    private void StopWalkingAnimation()
    {
        Animator.SetBool("IsWalking", false);
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
