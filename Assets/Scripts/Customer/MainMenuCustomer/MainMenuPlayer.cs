using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MainMenuPlayer : MonoBehaviour
{
    public GameObject OriginalSpawn;
    public GameObject cashMachineLookAt;
    public List<GameObject> Pathpoints;
    public NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float waitingTime;
    public bool isWaiting = false;
    [SerializeField] private int rdm;
    [SerializeField] private MainMenuManager_rework manager;

    private Quaternion originalRotation;
    private Quaternion currentRotation;
    private float t;
    private float speed = 0.1f;
    private bool haveToTurn;

    public void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        originalRotation = agent.transform.rotation;
    }

    private void Update()
    {
        t = t + Time.deltaTime;

        if(haveToTurn)
        {
            agent.transform.rotation = Quaternion.Lerp(currentRotation, originalRotation, t * speed);
        }
    }

    public IEnumerator Move()
    {
        rdm = Random.Range(0, Pathpoints.Count);
        agent.speed = 3;
        print(rdm);
        switch(rdm)
        {
            case 0:
                yield return new WaitForSeconds(3);
                animator.SetBool("isWalking", true);
                agent.SetDestination(Pathpoints[0].transform.position);
                StartCoroutine(Wait());
                break;
            case 1:
                yield return new WaitForSeconds(3);
                animator.SetBool("isWalking", true);
                agent.SetDestination(Pathpoints[1].transform.position);
                StartCoroutine(Wait());
                break;
            case 2:
                yield return new WaitForSeconds(3);
                animator.SetBool("isWalking", true);
                agent.SetDestination(Pathpoints[2].transform.position);
                StartCoroutine(Wait());
                break;
            case 3:
                yield return new WaitForSeconds(3);
                animator.SetBool("isWalking", true);
                agent.SetDestination(Pathpoints[3].transform.position);
                StartCoroutine(Wait());
                break;
        }
        yield return null;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.8f);
        animator.SetBool("isWalking", false);
        agent.transform.LookAt(Pathpoints[rdm].transform.position);
        yield return new WaitForSeconds(waitingTime);
        animator.SetBool("isWalking", true);
        agent.SetDestination(OriginalSpawn.transform.position);
        yield return new WaitForSeconds(1.5f);
        t = 0;
        agent.transform.LookAt(cashMachineLookAt.transform.position);
        haveToTurn = true;
        yield return new WaitForSeconds(0.5f);
        haveToTurn = false;
        animator.SetBool("isWalking", false);
        manager.LaunchLeavingFunction();
    }

    public void triggerAnimation(string trigger)
    {
        print("set trigger activé: " + trigger);
        animator.SetTrigger(trigger);
    }

}
