using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class NPC : MonoBehaviour
{
    private const float CHASE_SPEED = 2f;
    private const float WALK_SPEED = 1f;
    public static NPC currentNPC; // ���� �÷��̾�� ��ȣ�ۿ� ���� NPC

    private XRGrabInteractable grabInteractable;
    private Animator animator;
    private Rigidbody rigid;
    private NavMeshAgent nav;

    public Transform[] destinations;
    private Vector3 currentDestination;

    private NPC_MODE MODE;
    public NPC_MODE mode
    {
        get { return MODE; }
        set 
        {
            MODE = value;
            SetMode(MODE); 
        }
    }

    private bool isWalking;
    private bool isRePath;

    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        animator = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody>();
        nav = this.GetComponent<NavMeshAgent>();


        grabInteractable.trackPosition = grabInteractable.trackRotation = false;
        isWalking = false;
        isRePath = false;
        if (this.name.Contains("IDLE"))
            mode = NPC_MODE.IDLE;
        else if (this.name.Contains("CHASE"))
            mode = NPC_MODE.CHASE;
        else
            mode = NPC_MODE.RANDOM;
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    void Update()
    {
        switch (mode)
        {
            case NPC_MODE.IDLE:
                break;
            case NPC_MODE.CHASE:
                GotoTarget(PlayerCtrl.instance.transform.position);
                break;
            case NPC_MODE.RANDOM:
                if (CheckRemainingDistance() && !isRePath)
                {
                    isRePath = true;
                    isWalking = false;
                    SetAnimator();
                    Invoke("GotoTarget", 3f);
                }
                break;
        }
    }

    private void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private void SetAnimator()
    {
        animator.SetBool("isWalk", isWalking);
    }

    private bool CheckRemainingDistance()
    {
        return nav.remainingDistance < 0.01f;
    }

    private void SetMode(NPC_MODE _mode)
    {
        switch (_mode)
        {
            case NPC_MODE.IDLE:
                isWalking = false;
                nav.isStopped = true;
                break;
            case NPC_MODE.CHASE:
                isWalking = true;
                nav.speed = CHASE_SPEED;
                break;
            case NPC_MODE.RANDOM:
                isWalking = true;
                nav.speed = WALK_SPEED;
                GotoTarget();
                break;
        }

        SetAnimator();
    }

    private void GotoTarget()
    {
        GotoTarget(GetNextDestination());
    }

    private void GotoTarget(Vector3 _destination)
    {
        isWalking = true;
        SetAnimator();
        nav.SetDestination(_destination);
    }

    private Vector3 GetNextDestination()
    {
        Vector3 destination;

        do
        {
            destination = destinations[Random.Range(0, destinations.Length)].position;
            if (currentDestination == null)
                break;
        } while (destination == currentDestination);
        currentDestination = destination;
        isRePath = false;

        return destination;
    }
}
