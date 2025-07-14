using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(DamageAble))]
public class BlackWere : MonoBehaviour
{
    public float walkSpeed = 0.5f;
    Rigidbody2D rb;
    public float distance = 1f;
    public LayerMask layerMask;
    public Transform checkPoint;
    private Vector2 walkDirectionVector = Vector2.right;
    TouchingDirection touchingDirections;
    Animator animator;
    public DetectionZone attackZone;
    private float lockVelocityTimer = 0f;
    public float lockDuration = 0.3f;
    public float walkStopRate = 0.6f;
    DamageAble damageAble;
    private bool hasDied = false;
    public PlayerCon playerCon;
    private void Die()
    {
        if (hasDied) return; // Cegah dipanggil berkali-kali
        hasDied = true;

        AddPointsToPlayer(); // Tambah poin
        animator.SetTrigger("die"); // opsional: trigger animasi mati
        Destroy(gameObject, 0.5f); // Hapus objek setelah 0.5 detik
    }




    // Menambahkan variabel facingLeft untuk mengatur arah NPC
    private bool facingLeft = false;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageAble = GetComponent<DamageAble>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerCon = player.GetComponent<PlayerCon>();
        }

    }

    void Start()
    {
        // Pastikan playerCon terhubung di Inspector
        if (playerCon == null)
        {
            Debug.LogError("PlayerCon reference not set in BlackWere");
        }
    }

    // Update is called once per frame
    public enum WalkAbleDirection { Right, Left };

    private WalkAbleDirection _walkDirection;

    public WalkAbleDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkAbleDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkAbleDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }
    private void AddPointsToPlayer()
    {
        if (playerCon != null)
        {
            playerCon.AddPoints(1); // Menambahkan 10 poin, bisa disesuaikan sesuai kebutuhan
        }
    }


    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationString.hasTarget, value);
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkAbleDirection.Right)
        {
            WalkDirection = WalkAbleDirection.Left;
            facingLeft = true;
        }
        else if (WalkDirection == WalkAbleDirection.Left)
        {
            WalkDirection = WalkAbleDirection.Right;
            facingLeft = false;
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

        if (!damageAble.LockVelocity)
        {
            if (CanMove)
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(
                    Mathf.Lerp(rb.velocity.x, 0, walkStopRate),
                    rb.velocity.y);
        }

        // Patrol jalan terus ke arah hadap
        float direction = facingLeft ? -1f : 1f;
        transform.Translate(Vector2.right * direction * walkSpeed * Time.deltaTime);

        // Cek apakah ujung platform sudah habis
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);
        if (!hit)
        {
            FlipDirection();
        }
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        

        if (LockVelocity)
        {
            lockVelocityTimer -= Time.deltaTime;
            if (lockVelocityTimer <= 0f)
            {
                LockVelocity = false;
            }
        }
        if (!damageAble.IsAlive && !hasDied)
        {
            hasDied = true;
            Debug.Log("BlackWere mati, akan tambah poin");
            AddPointsToPlayer();
            Destroy(gameObject);
        }
    }

    public void Onhit(int damage, Vector2 knocks)
    {
        rb.velocity = new Vector2(knocks.x, rb.velocity.y + knocks.y);
        lockVelocityTimer = lockDuration;
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
    }
}
