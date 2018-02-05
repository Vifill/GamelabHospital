using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomSlotManager : MonoBehaviour 
{
    public List<WaitingSlotController> Slots;

    

    // Use this for initialization
    private void Start () 
	{
        Slots = new List<WaitingSlotController>(FindObjectsOfType<WaitingSlotController>());
	}
	
	// Update is called once per frame
	private void Update () 
	{
		
	}

    public List<WaitingSlotController> AvailableSlots ()
    {
        List<WaitingSlotController> temp = new List<WaitingSlotController>();

        foreach (WaitingSlotController Slot in Slots)
        {
            //if (Slot.IsReserved != true)
            //{
            //    temp.Add(Slot);
            //}
        }

        return temp;
    }
}
