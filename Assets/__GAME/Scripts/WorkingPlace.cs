using UnityEngine;

public class WorkingPlace: MonoBehaviour
{
    public string placeID;
    public float stoppingDistance = 0;

    public virtual Vector3 entryPosition => transform.position;
    public virtual bool isReady => false;    

    public void RobotLeave(Robot robot) 
    {
        robot.currentState = RobotState.Idle;
        OnRobotLeave(robot);
    }
    public void RobotArrive(Robot robot)
    {
        robot.currentState = RobotState.Working;
        Debug.Log($"Robot is at the {placeID}");
        OnRobotArrive(robot);

    }

    protected virtual void OnRobotArrive(Robot robot)
    {
    }
    protected virtual void OnRobotLeave(Robot robot)
    {
    }

    public static bool FindPlaceById(string placeID, out WorkingPlace workingPlace)
    {
        WorkingPlace[] places = FindObjectsByType<WorkingPlace>(FindObjectsSortMode.InstanceID);
        foreach (WorkingPlace place in places)
        {
            if (!place.isReady)
                continue;

            if (place.placeID.Equals(placeID, System.StringComparison.OrdinalIgnoreCase))
            {
                workingPlace = place;
                return true;
            }
        }
        workingPlace = null;
        return false;
    }
}
