using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(DamageAble))]
public class PlayerCon : MonoBehaviour
{
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 8f;
    private float lockVelocityTimer = 0f;
    public float lockDuration = 0.3f; // waktu player terkunci setelah kena hit
    private int playerPoints = 0;  // Poin pemain
    public TextMeshProUGUI scoreText;
    // Fungsi untuk menambah poin
    public void AddPoints(int points)
    {
        playerPoints += points;
        Debug.Log("Poin Pemain: " + playerPoints);  
        UpdateScoreUI(); // Memanggil fungsi untuk meng-update UI
    }

    // Fungsi untuk memperbarui UI dengan jumlah poin pemain
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Poin Pemain: " + playerPoints.ToString(); // Menampilkan poin di UI
        }
    }
    public float airWalkSpeed = 3f;
    TouchingDirection touchingDirections;

    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationString.lockVelocity); }
        set { animator.SetBool(AnimationString.lockVelocity, value); }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (isMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGround)
                    {
                        if (IsRunning) return runSpeed;
                        else return walkSpeed;
                    }
                    else return airWalkSpeed;
                }
                else return 0;
            }
            else return 0;
        }
    }

    [SerializeField]
    private bool _canMove = true;
    public bool CanMove
    {
        get { return _canMove; }
        private set
        {
            _canMove = value;
            animator.SetBool(AnimationString.canMove, value);
        }
    }

    public bool _IsFacingRight = true;
    public bool IsFacingRight
    {
        get { return _IsFacingRight; }
        private set
        {
            if (_IsFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1); // Membalik arah karakter
            }
            _IsFacingRight = value;
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value); // Pastikan animasi 'isMoving' aktif saat bergerak
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get { return _isRunning; }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value); // Pastikan animasi 'isRunning' aktif saat berlari
        }
    }

    Rigidbody2D rb;
    Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirection>();
    }

    void Update()
    {
        UpdateScoreUI();
        if (LockVelocity)
        {
            lockVelocityTimer -= Time.deltaTime;
            if (lockVelocityTimer <= 0f)
            {
                LockVelocity = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationString.yVelocity, rb.velocity.y);
    }

    // Mengambil input pergerakan dari Player (digunakan untuk gerakan kanan kiri)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Membaca input pergerakan
        if (CanMove)
        {
            isMoving = moveInput != Vector2.zero; // Aktifkan animasi 'isMoving' saat bergerak
            SetFacingDirection(moveInput); // Mengatur arah karakter
        }
        else
        {
            isMoving = false;
        }
    }

    // Mengatur pembalikan karakter (flip) berdasarkan input pergerakan
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight) IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight) IsFacingRight = false;
    }

    // Menangani aksi lari
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started) IsRunning = true;
        else if (context.canceled) IsRunning = false;
    }

    // Menangani aksi lompat
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGround && CanMove)
        {
            animator.SetTrigger(AnimationString.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
    public int GetPoints()
    {
        return playerPoints;
    }


    // Menangani aksi serangan
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationString.attack);
        }
    }

    // Menangani saat terkena damage
    public void Onhit(int damage, Vector2 knocks)
    {
        LockVelocity = true;
        rb.velocity = new Vector2(knocks.x, rb.velocity.y + knocks.y);
        lockVelocityTimer = lockDuration;
    }

    public void OnMoveSimulate(Vector2 direction)
    {
        moveInput = direction;
        if (CanMove)
        {
            isMoving = direction != Vector2.zero;
            SetFacingDirection(direction);
        }
        else
        {
            isMoving = false;
        }
    }

    public void OnJumpSimulate()
    {
        if (touchingDirections.IsGround && CanMove)
        {
            animator.SetTrigger(AnimationString.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttackSimulate()
    {
        animator.SetTrigger(AnimationString.attack);
    }
    public void OnMoveLefts()
    {
        OnMoveSimulate(Vector2.left);
    }
    public void OnMoveRight()
    {
        OnMoveSimulate(Vector2.right);
    }

    public void OnMoveZero()
    {
        OnMoveSimulate(Vector2.zero);
    }
    public void OnRunSimulate(bool isRunning)
    {
        IsRunning = isRunning;
    }


}
