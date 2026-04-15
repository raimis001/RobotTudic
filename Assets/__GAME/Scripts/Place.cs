using System.Collections.Generic;
using UnityEngine;

public class Place : WorkingPlace
{

    [SerializeField]
    List<RobotConsole> robotConsoles;

    public override Vector3 entryPosition => entry != null ? entry.position : base.entryPosition;
    public Transform entry;     

    protected override void OnRobotArrive(Robot robot)
    {
        foreach (RobotConsole console in robotConsoles)
        {
            if (!console.GetFreeSlot(out ConsoleSlot freeSlot))
                continue;

            robot.workingPlace = this;
            console.OccupySlot(freeSlot, robot);
            break;

        }
    }
    protected override void OnRobotLeave(Robot robot)
    {
        foreach (RobotConsole console in robotConsoles)
        {
            if (console.FreeSlotForRobot(robot))
                break;
        }

        robot.MoveToLocation(entry.position, () =>
        {
            robot.workingPlace = null;
            robot.currentState = RobotState.Idle;
            Debug.Log($"Robot has left the {placeID}");
        });
    }

    

}
