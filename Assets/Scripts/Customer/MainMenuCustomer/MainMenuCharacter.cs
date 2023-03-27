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

        StartCoroutine(waiting());
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, agent.destination) <= 1.5f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                triggerAnimation("Idle");
                player.StartCoroutine(player.Move());
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

    private IEnumerator waiting()
    {
        yield return new WaitForSeconds(waitingTime);
        agent.SetDestination(OriginalSpawn.transform.position);
        yield return new WaitForSeconds(2f);
        manager.currentCustomer = null;
        StartCoroutine(manager.spawnCustomer());
        Destroy(this.gameObject);
    }

    public void triggerAnimation(string trigger)
    {
        print("set trigger activé: " + trigger);
        animator.SetTrigger(trigger);
    }

}
