using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class Npc_2 : MonoBehaviour
{
    // public float walkSpeed = 3f;

    // Rigidbody2D rb;
    // TouchingDirection touchinDirection;

    // public enum WalkDirectionEnum { Right, Left}

    // private Vector2 WalkDirectionVector;
    // private WalkDirectionEnum _walkDirection;

    // public WalkDirectionEnum WalkDirection
    // {
    //     get { return _walkDirection; }
    //     set { 
    //         if(_walkDirection != value )
    //         {
    //             gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
    //
    //             if(value = WalkDirection.Right)
    //             {
    //                 walkDIrectionVector = Vector2.right;
    //             }else if(value == WalkDirection.Left)
    //             {
    //                 walkDIrectionVector = Vector2.left;
    //             }
    //         }
    //         
    //         
    //         _walkDirection = value; }
    // 
    // 
    // }
    //
    // private void Awake()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    //     touchinDirection = GetComponent<TouchingDirection>();
    //
    // }
    //
    // private void FixedUpdate()
    // {
    //     if (touchinDirection.IsGrounded && touchinDirection.IsOnWall)
    //     {
    //         FlipDirection();
    //     }
    //     rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.velocity.y);
    // }
    // void Start()
    // {
    //     
    // }
    // private void FlipDirection()
    // {
    //     if(WalkDirection == WalkableDirection.Right)
    //     {
    //         WalkDirection = WalkableDirection.Left;
    //     }else if(WalkDirection == WalkableDirection.Left)
    //     {
    //         WalkDirection = WalkableDirection.RIght;
    //     }
    //     else
    //     {
    //         Debug.LogError("Current Error");
    //     }
    // }
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }
}
