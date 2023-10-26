using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavControl : MonoBehaviour
{
    public GameObject target;
    public GameObject target1;
    private NavMeshAgent agent;
    bool isWalking = true;
    private Animator animator;

    public float rotateSpeed;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            agent.destination = target.transform.position;
        }
        else
        {
            agent.destination = transform.position;
            RotateTowardsTarget();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Red")
        {
            isWalking = false;
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Red")
        {
            isWalking = true;
            animator.SetTrigger("Walk");
        }
    }

    void RotateTowardsTarget()
    {
        float stepSize = rotateSpeed * Time.deltaTime;

        Vector3 targetDir = target1.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
