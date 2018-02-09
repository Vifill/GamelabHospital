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

    public List<BedStation> GetAvailableBeds()
    {
        //return Beds.Where(a => !a.IsReserved).ToList();
        List<BedStation> bedStations = new List<BedStation>();
        foreach (var bed in Beds.Where(a => !a.IsReserved))
        {
            bedStations.Add(bed.GetComponent<BedStation>());
        }
        return bedStations.OrderBy(x=>x.DirtyMeter).ToList();
        //return bedStations.Sort((x, y) => x.DirtyMeter.CompareTo(y.DirtyMeter));
    }
	
}
