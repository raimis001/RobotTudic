using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public enum RobotState
{
    Idle,
    Moving,
    Working,
    Charging
}

public static class RobotStates
{
    public static float energyConsumptionRate = 0.01f;
    public static float energyRechargeRate = 0.02f;
}

public class Robot : MonoBehaviour
{
    public float energy = 1;
    public Image eneryProgress;

    NavMeshAgent agent;

    [HideInInspector]
    public RobotState currentState = RobotState.Idle;

    float idleTime = 0f;
    internal WorkingPlace workingPlace;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (currentState != RobotState.Charging)
            energy -= Time.deltaTime * RobotStates.energyConsumptionRate;
        

        if (currentState == RobotState.Idle)
        {
            if (energy < 0.5f)
            {
                if (WorkingPlace.FindPlaceById("Campfire", out WorkingPlace place))
                {
                    MoveToLocation(place.entryPosition, () =>
                    {
                        place.RobotArrive(this);
                    });

                }
            }

            idleTime += Time.deltaTime;
            if (idleTime > 2)
            {
                currentState = RobotState.Moving;

                if (WorkingPlace.FindPlaceById("Tree", out WorkingPlace place))
                {
                    agent.stoppingDistance = place.stoppingDistance;
                    MoveToLocation(place.entryPosition, () =>
                    {

                        place.RobotArrive(this);
                    });
                }
                else
                {

                    MoveToRandomLocation();
                }
            }

        }

    }

    private void LateUpdate()
    {
        eneryProgress.fillAmount = energy;
    }

    void MoveToRandomLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection.y = 0;
        randomDirection += transform.position;
        MoveToLocation(randomDirection, () =>
        {
            currentState = RobotState.Idle;
            idleTime = 0;
        });

    }

    public void MoveToLocation(Vector3 destination, System.Action onArrival)
    {
        currentState = RobotState.Moving;
        agent.SetDestination(destination);
        StartCoroutine(CheckArrival(onArrival));
    }
    IEnumerator CheckArrival(System.Action onArrival)
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        onArrival?.Invoke();
    }
}
