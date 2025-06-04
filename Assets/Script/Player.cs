using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public int MaxHealt = 3;

    private float jumpHeight = 8f;
    private float moveSpeed = 5f;
    private bool facingRight = true;
    private bool isGround = true;
    private Vector2 moveInput;

    private PlayerController inputActions;

    private void OnEnable()
    {
        // Inisialisasi inputActions jika belum
        if (inputActions == null)
        {
            inputActions = new PlayerController();

            // Tambahkan event handler hanya sekali
            inputActions.Movement.Move.performed += OnMove;
            inputActions.Movement.Move.canceled += OnMoveCanceled;
            inputActions.Movement.Jump.performed += OnJump;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
        }
    }

    void Update()
    {
        // Flip karakter

        if (MaxHealt <= 0)
        {
            Die();
        }
        if (moveInput.x < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (moveInput.x > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // Animasi lari
        animator.SetFloat("Run", Mathf.Abs(moveInput.x));

        // Serangan (keyboard)
        if (Keyboard.current.rKey.isPressed)
        {
            animator.SetTrigger("Attack_1");
        }
        if (Keyboard.current.tKey.isPressed)
        {
            animator.SetTrigger("Attack_2");
        }
        if (Keyboard.current.yKey.isPressed)
        {
            animator.SetTrigger("Attack_3");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(moveInput.x, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }

    // Event handler input
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGround)
        {
            Jump();
            animator.SetBool("Jump", true);
            isGround = false;
        }
    }

    // UI Mobile Support
    public void OnJumpButton()
    {
        if (isGround)
        {
            Jump();
            animator.SetBool("Jump", true);
            isGround = false;
        }
    }

    public void OnAttack1Button()
    {
        animator.SetTrigger("Attack_1");
    }

    public void OnAttack2Button()
    {
        animator.SetTrigger("Attack_2");
    }

    public void OnMoveLeftButtonDown()
    {
        moveInput.x = -1f;
    }

    // Lepas tombol kiri
    public void OnMoveLeftButtonUp()
    {
        moveInput.x = 0f;
    }

    // Tekan tombol kanan
    public void OnMoveRightButtonDown()
    {
        moveInput.x = 1f;
    }

    // Lepas tombol kanan
    public void OnMoveRightButtonUp()
    {
        moveInput.x = 0f;
    }

    public void OnMoveButtonUp()
    {
        moveInput.x = 0f;
    }

    public void TakeDamage(int damage)
    {
        if(
            MaxHealt <= 0)
        {
            return;
        }
        MaxHealt -= damage;
    }

    void Die()
    {
        Debug.Log("Player mati");
    }
}
