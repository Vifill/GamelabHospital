using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseInputController : MonoBehaviour 
{

    private List<OrderlyController> Orderlies;
    private Queue<OrderlyOrder> Orders = new Queue<OrderlyOrder>();

	// Use this for initialization
	private void Start () 
	{
        Orderlies = new List<OrderlyController>(FindObjectsOfType<OrderlyController>());
	}
	
	// Update is called once per frame
	private void Update () 
	{
        CheckOrderQueue();
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


                    AddOrderToQueue(order);
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
    }

    private void AddOrderToQueue(OrderlyOrder pOrder)
    {
        Orders.Enqueue(pOrder);
        CheckOrderQueue();
        Orderlies[0].InitializeQueueUI();
    }

    private void CheckOrderQueue()
    {
        if(Orders.Count > 0)
        {
            var nextOrder = Orders.First();
            var availableOrderly = GetNearestAvailableOrderly(nextOrder);
            availableOrderly?.StartOrder(Orders.Dequeue());
        }
    }

    private OrderlyController GetNearestAvailableOrderly(OrderlyOrder pNextOrder)
    {
        return Orderlies.Where(a => a.CurrentOrder == null).OrderBy(a=> Vector3.Distance(a.transform.position, pNextOrder.FirstActionPosition)).FirstOrDefault();
    }

    public void ClearQueue()
    {
        Orders.Clear();
        Orderlies[0].InitializeQueueUI();
    }

    public List<OrderlyInteractionAction> GetAllInteractionActions()
    {
        return Orders.Select(a => a.GetInteractionAction()).ToList();
    }
}
