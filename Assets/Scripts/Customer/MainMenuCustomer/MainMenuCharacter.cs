using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainMenuCharacter : MonoBehaviour
{
    public GameObject OriginalSpawn;
    public GameObject PathPoint;
    public NavMeshAgent agent;
    public MainMenuPlayer player;
    public MainMenuManager_rework manager;
    [SerializeField] private Animator animator;
    [SerializeField] private float waitingTime = 80;
    public bool isWaiting = false;

    public void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
        OriginalSpawn = GameObject.FindGameObjectWithTag("MM_spawn");
        PathPoint = GameObject.FindGameObjectWithTag("MM_Path");
        player = GameObject.FindObjectOfType<MainMenuPlayer>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        manager = FindObjectOfType<MainMenuManager_rework>();

        agent.SetDestination(PathPoint.transform.position);
        agent.speed = 3;
        triggerAnimation("Walk");
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, agent.destination) <= 1.5f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                triggerAnimation("Idle");
                StartCoroutine(checkIfWaiting());
            }
        }
        else if (Vector3.Distance(transform.position, agent.destination) > 1.5f)
        {
            if (isWaiting)
            {
                isWaiting = false;
                triggerAnimation("Walk");
            }
        }
    }

    private IEnumerator HelloAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Happy");
    }

    private IEnumerator checkIfWaiting()
    {
        yield return new WaitForSeconds(0.5f);
        if(isWaiting)
        {
            StartCoroutine(player.Move());
            StartCoroutine(HelloAnimation());
        }
    }


    public IEnumerator Leaving()
    {
        if (manager != null)
        {
            print("leaving function");
            triggerAnimation("Happy");
            yield return new WaitForSeconds(2f);
            manager.currentCustomer = null;
            StartCoroutine(manager.spawnCustomer());
            Destroy(this.gameObject);
        }
        else
        {
            SearchManager();
        }
    }

    private void SearchManager()
    {
        manager = FindObjectOfType<MainMenuManager_rework>();
        manager.ActiveCustomer();
        StartCoroutine(Leaving());
    }

    public void triggerAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }

}
