using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseInputController : MonoBehaviour 
{
    [HideInInspector]
    public List<OrderlyController> Orderlies;
    [HideInInspector]
    public OrderlyController CurrentOrderly;

	// Use this for initialization
	public virtual void Start () 
	{
        Orderlies = new List<OrderlyController>(FindObjectsOfType<OrderlyController>());
        if(Orderlies.Any())
        {
            SelectOrderly(Orderlies[0]);
        }
    }
	
	// Update is called once per frame
	public virtual void Update () 
	{
        if (Input.GetMouseButtonDown(0) && !GameController.InMenuScreen)
        {
            OrderlyOrder order = GetOrderFromMouse();
            if(order != null)
            {
                CurrentOrderly.AddQueue(order);
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
            CancelOrder();
        }
    }

    protected virtual void CancelOrder()
    {
        CurrentOrderly?.CurrentAction?.CancelOrder();
    }

    protected OrderlyOrder GetOrderFromMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            var actionable = hit.transform.root.GetComponent<Actionable>();
            if (actionable != null && actionable.IsActionActive)
            {
                var order = new OrderlyOrder(actionable.transform.position);
                order.AddAction(new OrderlyMoveAction(actionable.transform));
                order.AddAction(new OrderlyInteractionAction(actionable));
                return order;
            }
            else if (hit.transform.tag == "Floor")
            {
                var order = new OrderlyOrder(hit.point);
                order.AddAction(new OrderlyMoveAction(null, hit.point, 0, 1.1f));
                return order;
            }
            Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
        }
        return null;
    }

    private void SelectOrderly(OrderlyController pOrderlyController)
    {
        CurrentOrderly?.SelectionParticleEffect.SetActive(false);
        CurrentOrderly = pOrderlyController;
        pOrderlyController.SelectionParticleEffect.SetActive(true);
    }
}
