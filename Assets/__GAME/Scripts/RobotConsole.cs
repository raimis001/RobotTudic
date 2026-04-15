using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConsoleSlot
{
    public Transform slotTransform;
    public bool isOccupied;
    public Robot robot;
}

public class RobotConsole : MonoBehaviour
{
    [SerializeField]
    List<ConsoleSlot> slots;

    public bool GetFreeSlot(out ConsoleSlot freeSlot)
    {
        foreach (ConsoleSlot slot in slots)
        {
            if (!slot.isOccupied)
            {
                freeSlot = slot;
                return true;
            }
        }

        freeSlot = null;
        return false;
    }

    public void OccupySlot(ConsoleSlot slot, Robot robot)
    {
        slot.isOccupied = true;
        slot.robot = robot;

        robot.MoveToLocation(slot.slotTransform.position, () =>
        {
            robot.currentState = RobotState.Charging;
            RobotStates.energyRechargeRate = 0.03f;
        });
    }

    internal bool FreeSlotForRobot(Robot robot)
    {
        foreach (ConsoleSlot slot in slots)
        {
            if (slot.robot == robot)
            {
                slot.isOccupied = false;
                slot.robot = null;
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        foreach (ConsoleSlot slot in slots)
        {
            if (slot.isOccupied && slot.robot != null)
            {
                // Simulate energy recharge
                slot.robot.energy += RobotStates.energyRechargeRate * Time.deltaTime;
                if (slot.robot.energy >= 1)
                {
                    slot.robot.energy = 1f;
                    slot.robot.workingPlace.RobotLeave(slot.robot);
                }
            }
        }
    }

}
