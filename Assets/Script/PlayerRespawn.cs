using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 lastCheckpointPosition;
    private Rigidbody2D rb;
    private DamageAble damageAble;

    public float fallThresholdY = -10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageAble = GetComponent<DamageAble>();
        lastCheckpointPosition = transform.position;
    }

    void Update()
    {
        // Jatuh ke bawah
        if (transform.position.y < fallThresholdY)
        {
            Respawn();
        }

        // Mati karena musuh
        if (!damageAble.IsAlive)
        {
            Respawn();
        }
    }

    public void SetCheckpoint(Vector2 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        Debug.Log("Checkpoint Tersimpan di: " + checkpointPosition);
    }

    public void Respawn()
    {
        Debug.Log("Respawn ke checkpoint");

        // Reset posisi & kecepatan
        transform.position = lastCheckpointPosition;
        rb.velocity = Vector2.zero;

        // Reset HP player
        damageAble.Health = damageAble.MaxHealth;
        damageAble.IsAlive = true;

        // Reset animasi mati
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool(AnimationString.IsAlive, true);
        }
    }
}
