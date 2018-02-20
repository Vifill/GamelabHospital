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

        if (CanMove)
        {
            //CharCntrlr Move

            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            {
                //horizontal = Mathf.Lerp(horizontal, Input.GetAxisRaw("Horizontal"), WindUpTime * Time.deltaTime);
                //vertical = Mathf.Lerp(vertical, Input.GetAxisRaw("Vertical"), WindUpTime * Time.deltaTime);

                //Debug.Log("H " + horizontal + "  V " + vertical);

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
                        Animator.SetBool("IsWalking", true);
                        EmissionModule.enabled = true;
                    }
                }
            }
            else
            {
                if (Animator.GetBool("IsWalking"))
                {
                    Animator.SetBool("IsWalking", false);
                    EmissionModule.enabled = false;
                }
            }
            //if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            //{
            //    Velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //    RB.velocity = Velocity * Time.deltaTime * Speed;
            //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Velocity), RotSpeed * Time.deltaTime);
            //}
            //else
            //{
            //    RB.velocity = Vector3.zero;
            //}



            //CharCntrlr SimpleMove
            //Velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //transform.GetComponent<CharacterController>().SimpleMove(Velocity * Time.deltaTime * Speed);

            //if (Velocity.sqrMagnitude != 0)
            //{
            //    transform.rotation = Quaternion.LookRotation(Velocity);
            //}
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

    public void StopMovement()
    {
        CanMove = false;
    }

    public void StartMovement()
    {
        CanMove = true;
    }
}
