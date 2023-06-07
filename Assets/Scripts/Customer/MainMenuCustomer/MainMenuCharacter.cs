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
                StartCoroutine(HelloAnimation());
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
        animator.SetBool("Talk 0", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Talk");
        yield return new WaitForSeconds(1f);
        animator.SetBool("Talk 0", false);
    }

    private IEnumerator checkIfWaiting()
    {
        yield return new WaitForSeconds(0.5f);
        if(isWaiting)
        {
            StartCoroutine(player.Move());
        }
    }



    public void triggerAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }

}
