using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{

    public string placeID;
    [SerializeField]
    List<RobotConsole> robotConsoles;
    [SerializeField]
    Transform entry;


    public static Place FindPlaceById(string placeID, out Transform entry)
    {
        Place[] places = FindObjectsByType<Place>(FindObjectsSortMode.InstanceID);
        foreach (Place place in places)
        {
            if (place.placeID.Equals(placeID,System.StringComparison.OrdinalIgnoreCase))
            {
                entry = place.entry;
                return place;
            }
        }
        entry = null;
        return null;
    }
}
