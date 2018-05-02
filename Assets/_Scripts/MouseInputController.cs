using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseInputController : MonoBehaviour 
{
    private List<OrderlyController> Orderlies;
    private OrderlyController CurrentOrderly;

	// Use this for initialization
	private void Start () 
	{
        Orderlies = new List<OrderlyController>(FindObjectsOfType<OrderlyController>());
        if(Orderlies.Any())
        {
            SelectOrderly(Orderlies[0]);
        }
    }
	
	//Update is called once per frame
	private void Update ()
	{
        if (Input.GetMouseButtonDown(0) && !GameController.InMenuScreen)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {                
                var actionable = hit.transform.root.GetComponent<Actionable>();
                if(actionable != null && actionable.IsActionActive)
                {
                    var order = new OrderlyOrder(actionable.transform.position);
                    order.AddAction(new OrderlyMoveAction(actionable.transform));
                    order.AddAction(new OrderlyInteractionAction(actionable));

                    CurrentOrderly.AddQueue(order);
                }
                Debug.Log("You selected the " + hit.transform.name); //ensure you picked right object
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectOrderly(Orderlies[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectOrderly(Orderlies[1]);
        }

        if (Input.GetMouseButtonDown(1) && !GameController.InMenuScreen)
        {
            CurrentOrderly?.CurrentAction?.CancelOrder();
        }
    }

    private void SelectOrderly(OrderlyController pOrderlyController)
    {
        CurrentOrderly?.SelectionParticleEffect.SetActive(false);
        CurrentOrderly = pOrderlyController;
        pOrderlyController.SelectionParticleEffect.SetActive(true);
    }
}
