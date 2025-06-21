using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class BlackWere : MonoBehaviour
{
    // Start is called before the first frame update

    public float walkSpeed = 3f;
    Rigidbody2D rb;
    private Vector2 walkDirectionVector = Vector2.right;
    TouchingDirection touchingDirections;
    Animator animator;
    public DetectionZone attackZone;
    public float walkStopRate = 0.6f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Optional: tambahkan sesuatu atau biarkan kosong
    }



    // Update is called once per frame

    public enum WalkAbleDirection { Right, Left};

    private WalkAbleDirection _walkDirection;

    public WalkAbleDirection WalkDirection
    {
        get{ return _walkDirection; }
        set {
            if (_walkDirection != value)
            {

                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkAbleDirection.Right)
                {
                    walkDirectionVector = Vector2.right;

                }else if(value == WalkAbleDirection.Left){
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value; }
    }
    public bool _hasTarget = false;
    public bool HasTarget { 
        
        get { return _hasTarget; } 
        private set {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);
        } }

    private void FlipDirection()
    {
        if (WalkDirection == WalkAbleDirection.Right)
        {
            WalkDirection = WalkAbleDirection.Left;
        }else if(WalkDirection == WalkAbleDirection.Left)
        {
             WalkDirection = WalkAbleDirection.Right;
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationString.canMove);
        }
    }
    private void FixedUpdate()
    {

        if (touchingDirections.IsOnWall && touchingDirections.IsGround)
        {
            FlipDirection();
        }
        if(CanMove)
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        else
            rb.velocity = new Vector2(
            Mathf.Lerp(rb.velocity.x, 0, walkStopRate),
            rb.velocity.y);

    }
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }
}
