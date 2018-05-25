using System.Collections;
using System.Collections.Generic;
using Assets._Scripts.Utilities;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrollingAI : MonoBehaviour
{
    public List<GameObject> Models;
    public List<Transform> WayPoints;
    public bool ChangeSkinOnRepath;

    private NavMeshAgent Agent;
    private Animator Animator;
    private bool isWaiting;
    private int WaypointIndex = 0;
    private int ModelIndex = 0;


    void Start ()
	{
	    Agent = GetComponent<NavMeshAgent>();
	    Animator = GetComponentInChildren<Animator>();
	}
	
	void Update ()
	{
	    if (!Agent.hasPath && !isWaiting && WayPoints != null)
	    {
	        Animator?.SetBool(Constants.AnimationParameters.CharacterIsWalking, false);
            WaypointIndex++;
	        if (WaypointIndex >= WayPoints.Count)
	        {
	            WaypointIndex = 0;
	        }

	        StartCoroutine(DelayedPathRecalculate());
	    }
	}

    IEnumerator DelayedPathRecalculate()
    {
        isWaiting = true;
        if (ChangeSkinOnRepath && Models.Count != 0)
        {
            ModelIndex++;
            if (ModelIndex >= Models.Count)
            {
                ModelIndex = 0;
            }
            Destroy(transform.GetChild(0).gameObject);
            GameObject GO = (GameObject)Instantiate(Models[ModelIndex], transform);
            GO.transform.localPosition = -Vector3.up;
            Animator = GO.GetComponentInChildren<Animator>();
        }
        yield return new WaitForSeconds(Random.Range(1,10));
        Agent.SetDestination(WayPoints[WaypointIndex]?.position ?? Vector3.up);
        Animator?.SetBool(Constants.AnimationParameters.CharacterIsWalking, true);
        isWaiting = false;
    }
}
