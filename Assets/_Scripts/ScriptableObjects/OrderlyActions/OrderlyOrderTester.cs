using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderlyOrderTester : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            var orderlyController = FindObjectOfType<OrderlyController>();
            var station = FindObjectsOfType<PickupStationController>()[0];
            var stationPos = station.transform.position;
            var table = FindObjectsOfType<TableStation>()[0];
            var tablePos = table.transform.position;

            OrderlyOrder order = new OrderlyOrder(stationPos);
            order.AddAction(new OrderlyMoveAction(station.transform));
            order.AddAction(new OrderlyInteractionAction(station));
            order.AddAction(new OrderlyMoveAction(table.transform));
            order.AddAction(new OrderlyInteractionAction(table));

            orderlyController.StartOrder(order);
        }
	}
}
