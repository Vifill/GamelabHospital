using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class BedManager : ScriptableObject 
{
    public List<BedController> Beds;
    
    public void InitializeBeds()
    {
        Beds = FindObjectsOfType<BedController>().ToList();
    }

    public List<BedController> GetAvailableBeds()
    {
        return Beds.Where(a => !a.IsReserved).ToList();
    }
}
