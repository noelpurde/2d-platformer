using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public DetectionZone wingDetectionZone;
    public List<Transform> waypoints;
    public float waypointReachedDistance = 0.1f;

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWayPoint;
    int wayPointNum = 0;

    public bool _hasTarget = false; 

    public bool HasTarget { get { return _hasTarget; } private set
    {
        _hasTarget = value;
        animator.SetBool(AnimationStrings.hasTarget, value);
    }
    }

    public bool CanMove
    {
        get 
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        nextWayPoint = waypoints[wayPointNum];
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = wingDetectionZone.detectedColliders.Count > 0;    
    }

    void FixedUpdate()
    {
        if(damageable.IsAlive)
        {
            if(CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        Vector2 directionToWaypoint = (nextWayPoint.position - transform.position). normalized;

        float distance = Vector2.Distance(nextWayPoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;

        UpdateDirection();

        if(distance <= waypointReachedDistance)
        {
            wayPointNum++;

            if(wayPointNum >= waypoints.Count)
            {
                wayPointNum = 0;
            }

            nextWayPoint = waypoints[wayPointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if(transform.localScale.x > 0)
        {
            if(rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if(rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }
}
