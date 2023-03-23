using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MainMenuPlayer : MonoBehaviour
{
    public GameObject OriginalSpawn;
    public List<GameObject> Pathpoints;
    public NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float waitingTime = 80;
    public bool isWaiting = false;

    public void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }



    public IEnumerator Move()
    {
        int rdn = Random.Range(0, Pathpoints.Count);
        agent.speed = 3;
        switch(rdn)
        {
            case 0:
                agent.SetDestination(Pathpoints[0].transform.position);
                StartCoroutine(Wait());
                break;
            case 1:
                agent.SetDestination(Pathpoints[1].transform.position);
                StartCoroutine(Wait());
                break;
            case 2:
                agent.SetDestination(Pathpoints[2].transform.position);
                StartCoroutine(Wait());
                break;
            case 3:
                agent.SetDestination(Pathpoints[3].transform.position);
                StartCoroutine(Wait());
                break;
        }

        yield return new WaitForSeconds(waitingTime);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitingTime);
        agent.SetDestination(OriginalSpawn.transform.position);
    }

    public void triggerAnimation(string trigger)
    {
        print("set trigger activé: " + trigger);
        animator.SetTrigger(trigger);
    }

}
