using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public int maxHealth = 100;
    public int currentHealth;
    public TMP_Text darah;
    public Slider healthSlider;
    public int totalNpcKilled = 0;  
    public int requiredNpcKills = 10;  
    private float jumpHeight = 8f;
    private float moveSpeed = 5f;
    private bool facingRight = true;
    private bool isGround = true;
    private Vector2 moveInput;
    // Dialog variables
    public GameObject dialogPanel;  // Panel dialog
    public TMP_Text dialogText;     // Tempat teks dialog muncul
    public string[] dialogLines;    // Baris dialog
    private int currentLine = 0;    // Indeks baris dialog yang ditampilkan
    private bool isDialogActive = false;    // Menyimpan status dialog aktif atau tidak

    private PlayerController inputActions;
    void Start()
    {
        currentHealth = maxHealth; // Set nilai currentHealth sama dengan maxHealth
    }


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


        if (currentHealth <= 0)
        {
            
                Die(); // Panggil animasi mati
            
        }
        darah.text = currentHealth.ToString();


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

        // Jika tombol dialog ditekan (contoh: tombol E untuk keyboard)
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TriggerDialog();
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(moveInput.x, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    public void EndDialog()
    {
        dialogPanel.SetActive(false); // Menyembunyikan panel dialog setelah selesai
        isDialogActive = false;
        currentLine = 0;
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
    public void OnTriggerDialogButton()
    {
        TriggerDialog();
    }

    public void TriggerDialog()
    {
        if (dialogLines != null && dialogLines.Length > 0)  // Pastikan ada dialog yang diatur
        {
            if (!isDialogActive)
            {
                dialogText.text = dialogLines[currentLine];
                dialogPanel.SetActive(true); // Menampilkan panel dialog
                isDialogActive = true;
            }
            else
            {
                // Lanjutkan dialog ke baris berikutnya
                currentLine++;
                if (currentLine < dialogLines.Length)
                {
                    dialogText.text = dialogLines[currentLine];
                }
                else
                {
                    EndDialog();
                }
            }
        }
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
        Debug.Log($"Damage diterima: {damage}. Kesehatan sebelum: {currentHealth}");

        if (currentHealth <= 0)
        {
            return;  // Jika darah sudah habis, tidak ada damage yang akan diterima
        }

        currentHealth -= damage;

        Debug.Log($"Kesehatan setelah damage: {currentHealth}");

        // Update UI health
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player mati");

        // Trigger animasi mati
        
    }
    IEnumerator WaitAndDisable()
    {
        // Tunggu selama durasi animasi mati (misalnya 3 detik, sesuaikan dengan durasi animasi)
        yield return new WaitForSeconds(3f);

        // Setelah animasi selesai, matikan objek player
        gameObject.SetActive(false);
    }

}