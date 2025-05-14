using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Rigidbody2D rb;
    private float jumpHeight = 8f;
    private bool isGround = true;
    private float movement;
    private float moveSpeed = 5f;
    private bool facingRight = true;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        if(movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight= false;
        }else if(movement >0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
         
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        if(Mathf.Abs(movement) > 0f)
        {
            animator.SetFloat("Run",1f);
        }else if( movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }
        if (Input.GetKey(KeyCode.R)){
            animator.SetTrigger("Attack_1");
        }
        if (Input.GetKey(KeyCode.T))
        {
            animator.SetTrigger("Attack_2");
        }
        if (Input.GetKey(KeyCode.Y))
        {
            animator.SetTrigger("Attack_1");
        }

    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

     void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }


}
