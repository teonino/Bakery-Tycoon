using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainMenuCharacter : MonoBehaviour
{
    public GameObject OriginalSpawn;
    public GameObject PathPoint;
    public NavMeshAgent agent;
    [SerializeField] private float waitingTime = 80;

    private void OnEnable()
    {
        OriginalSpawn = GameObject.FindGameObjectWithTag("MM_spawn");
        PathPoint = GameObject.FindGameObjectWithTag("MM_Path");
        agent = gameObject.GetComponent<NavMeshAgent>();

        agent.SetDestination(PathPoint.transform.position);
        agent.speed = 3;

        StartCoroutine(waiting());
    }

    private IEnumerator waiting()
    {
        yield return new WaitForSeconds(waitingTime);
        agent.SetDestination(OriginalSpawn.transform.position);
    }

}
