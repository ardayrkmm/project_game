using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_1 : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Transform checkPoint;
    public float distance = 1f;
    public bool facingLeft = true;
    public LayerMask layerMask;
    public bool inRange = false;
    public Transform player;
    public float attackRange = 10f;
    public float retrieveDistance = 2.5f;
    public float chasespeed = 10f;
    public Animator animator;
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    void Update()
    {
        // Cek apakah player dalam jarak serang
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        inRange = distanceToPlayer <= attackRange;

        if (inRange)
        {
            // Cek arah player dan balikkan NPC jika perlu
            if (player.position.x > transform.position.x && facingLeft)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && !facingLeft)
            {
                Flip();
            }

            if (distanceToPlayer > retrieveDistance)
            {
                animator.SetBool("Attack_1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chasespeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack_1", true);
            }
        }
        else
        {
            // Patrol jalan terus ke arah hadap
            float direction = facingLeft ? -1f : 1f;
            transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

            // Cek apakah ujung platform sudah habis
            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);
            if (!hit)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Attack()
    {
       Collider2D  collifon = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collifon ) { 
        if(collifon.gameObject.GetComponent<Player>() != null)
            {
                collifon.gameObject.GetComponent<Player>().TakeDamage(1);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
