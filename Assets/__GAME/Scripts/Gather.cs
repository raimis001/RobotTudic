using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Gather :  WorkingPlace
{
    public int hits = 3;
    public float gatherTime = 2f;

    public List<ResourceAmount> resources;

    public override bool isReady => assignedRobot == null && !isDed;

    Robot assignedRobot;

    float gatherTimer;
    bool isDed = false;

    protected override void OnRobotArrive(Robot robot)
    {
        if (assignedRobot != null)
            return;

        assignedRobot = robot;
        assignedRobot.currentState = RobotState.Working;
    }
    protected override void OnRobotLeave(Robot robot)
    {
        assignedRobot.currentState = RobotState.Idle;
        assignedRobot.workingPlace = null;
        assignedRobot = null;
        Destroy(gameObject, 2);
    }




    void Update()
    {
        if (!assignedRobot)
            return;

        if (hits <= 0)
        {
            isDed = true;
            RobotLeave(assignedRobot);
            return;
        }

        Working();
    }

    void Working()
    {
        if (gatherTimer < gatherTime)
        {
            gatherTimer += Time.deltaTime;
            return;
        }

        gatherTimer = 0;
        hits -= 1;

        foreach (var resource in resources)
        {
            // Here you would add the gathered resources to the robot's inventory or similar
            Debug.Log($"Gathered {resource.amount} of {resource.ID}");
        }

    }

    
}
