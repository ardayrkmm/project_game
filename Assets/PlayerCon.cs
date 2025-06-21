using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(DamageAble))]
public class PlayerCon: MonoBehaviour
{
    // Start is called before the first frame update

    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 8f;
    private float lockVelocityTimer = 0f;
    public float lockDuration = 0.3f; // waktu player terkunci setelah kena hit

    public float airWalkSpeed = 3f;
    TouchingDirection touchingDirections;

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationString.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationString.lockVelocity, value);
        }

    }

    public float CurrentMoveSpeed
    {
        get
        {


            if (CanMove) {
                if (isMoving && !touchingDirections.IsOnWall)

                {
                    if (touchingDirections.IsGround)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }

                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }


        }
    }
    [SerializeField]
    private bool _canMove = true;

    public bool CanMove
    {
        get
        {
            return _canMove;
        }
        private set
        {
            _canMove = value;
            animator.SetBool(AnimationString.canMove, value);
        }

    }

    public bool _IsFacingRight = true;
    public bool IsFacingRight { get
        {
            return _IsFacingRight;
        }
        private set {
            if(_IsFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _IsFacingRight = value;
        } }
    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving { get { 
        return _isMoving;
        } private set {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        } }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value);
        }
       
       
    }
  
    Rigidbody2D rb;
    Animator animator;
    void Start()
    {

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirection>();
 
    }

    // Update is called once per frame
    void Update()
    {
        if (LockVelocity)
        {
            lockVelocityTimer -= Time.deltaTime;
            if (lockVelocityTimer <= 0f)
            {
                LockVelocity = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    private bool isInvicible = false;
    private float timeSinceHit = 0;
    public float inivicibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive; // Assuming _isAlive is the correct boolean representing the alive state
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationString.IsAlive, _isAlive); // Set the animation based on the value of _isAlive
        }
    }



    private void FixedUpdate()
    {

        if(!LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationString.yVelocity, rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if(IsAlive)
        {
            isMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            isMoving = false;
        }
        
    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if( moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGround && CanMove)
        {
            animator.SetTrigger(AnimationString.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

 
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationString.attack);
        }
    }

    public void Onhit(int damage, Vector2 knocks)
    {
        LockVelocity = true;
        rb.velocity = new Vector2(knocks.x, rb.velocity.y + knocks.y);
        lockVelocityTimer = lockDuration; 
    }
}
