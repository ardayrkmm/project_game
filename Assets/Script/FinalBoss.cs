using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(DamageAble))]
public class FinalBoss : MonoBehaviour
{
    public float walkSpeed = 5f;
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

    private GameObject spawnedChest; // Tambahkan ini di class FinalBoss

    public GameObject chestObject; // simpan referensi chest yang muncul

    private void Die()
    {
        if (hasDied) return;
        hasDied = true;

        Debug.Log("Boss mati - Memanggil Die()");

        AddPointsToPlayer();
        animator.SetTrigger("die");

        // Munculkan chest yang sudah ada di scene
        if (chestObject != null)
        {
            chestObject.SetActive(true); // aktifkan saat boss mati
            Debug.Log("Chest diaktifkan dari scene");
        }
        else
        {
            Debug.LogWarning("chestObject belum di-assign di Inspector!");
        }

        Destroy(gameObject, 0.5f);
    }

    private void ShowChest()
    {
        if (spawnedChest != null)
        {
            spawnedChest.SetActive(true);
            Debug.Log("Chest ditampilkan setelah delay");
        }
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
    public float attackRange = 2f;
    public float chaseRange = 10f;
        private float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    void Start()
    {
        // Pastikan playerCon terhubung di Inspector
        if (playerCon == null)
        {
            Debug.LogError("PlayerCon reference not set in BlackWere");
        }
        WalkDirection = WalkAbleDirection.Left;
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

        // Serang kalau dalam attackZone
        if (HasTarget && attackTimer <= 0f)
        {
            animator.SetTrigger("attack");
            attackTimer = attackCooldown;
        }

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        // Lock velocity timer
        if (LockVelocity)
        {
            lockVelocityTimer -= Time.deltaTime;
            if (lockVelocityTimer <= 0f)
            {
                LockVelocity = false;
            }
        }

        // Jika masih hidup, kejar player
        if (!hasDied && damageAble.IsAlive)
        {
            ChasePlayer();
        }

        // Mati
        if (!damageAble.IsAlive && !hasDied)
        {
            Die();
        }
    }
    private void ChasePlayer()
    {
        if (playerCon == null || !damageAble.IsAlive) return;

        float distanceToPlayer = playerCon.transform.position.x - transform.position.x;

        if (Mathf.Abs(distanceToPlayer) < chaseRange && Mathf.Abs(distanceToPlayer) > attackRange)
        {
            // Gerak ke arah player
            if (distanceToPlayer > 0 && WalkDirection != WalkAbleDirection.Right)
            {
                WalkDirection = WalkAbleDirection.Right;
            }
            else if (distanceToPlayer < 0 && WalkDirection != WalkAbleDirection.Left)
            {
                WalkDirection = WalkAbleDirection.Left;
            }

            if (CanMove && !damageAble.LockVelocity)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
        }
        else
        {
            // Berhenti kalau terlalu dekat
            rb.velocity = new Vector2(0, rb.velocity.y);
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
