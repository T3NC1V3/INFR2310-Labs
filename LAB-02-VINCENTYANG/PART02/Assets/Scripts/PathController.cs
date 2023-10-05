using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathController : MonoBehaviour
{
    [SerializeField] public PathManager pathManager;

    List<Waypoint> thePath;
    Waypoint target;

    public float moveSpeed;
    public float rotateSpeed;

    public Animator animator;
    bool isWalking;

    // Start is called before the first frame update
    void Start()
    {
        isWalking = false;
        animator.SetBool("isWalking", isWalking);

        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            // Set initial target to first waypoint
            target = thePath[0];
        }
    }

    void RotateTowardsTarget()
    {
        float stepSize = rotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void MoveForward()
    {
        float stepSize = Time.deltaTime * moveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget < stepSize)
        {
            // we will overshoot, do something else
            return;
        }

        // Take a step forward
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            // Toggle on key down
            isWalking = !isWalking;
            animator.SetBool("isWalking", isWalking);
        }

        if (isWalking)
        {
            RotateTowardsTarget();
            MoveForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Switch to next target
        target = pathManager.GetNextTarget();

        if (other.gameObject.tag == "Player")
        {
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);
            }
        }
    }
}
