using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum RobotState
{
    Idle,
    Moving,
    Working
}

public static class RobotStates
{
    public static float energyConsumptionRate = 0.01f;
}

public class Robot : MonoBehaviour
{
    public float energy = 1;
    public Image eneryProgress;

    NavMeshAgent agent;
    RobotState currentState = RobotState.Idle;

    float idleTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        energy -= Time.deltaTime * RobotStates.energyConsumptionRate;

        if (currentState == RobotState.Idle)
        {
            if (energy < 0.5f)
            { 
                Place place = Place.FindPlaceById("Campfire", out Transform entry);
                MoveToLocation(entry.position, () =>
                {
                    currentState = RobotState.Working;
                    Debug.Log("Robot is recharging at the campfire.");
                });
            }

            idleTime += Time.deltaTime;
            if (idleTime > 2)
            {
                currentState = RobotState.Moving;

                MoveToRandomLocation();
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

    void MoveToLocation(Vector3 destination, System.Action onArrival)
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
