using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    // Start is called before the first frame update
    public ContactFilter2D castFilter;
    public float wallDistance = 0.2f;
    public float cellingDistance = 0.05f;

    CapsuleCollider2D touchinCol;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] cellingHits = new RaycastHit2D[5];
    public float GroundDIstance = 0.1f;

    Animator animator;

    [SerializeField]
    private bool _isGround = true;
    public bool IsGround { get {
            return _isGround;
        }
        
        private set {
            _isGround = value;

            if (animator != null)
            {
                animator.SetBool(AnimationString.isGrounded, value);
            }

        } }

    [SerializeField]
    private bool _IsOnWall = true;

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;


    public bool IsOnWall
    {
        get
        {
            return _IsOnWall;
        }

        private set
        {
            _IsOnWall = value;

            if (animator != null)
            {
                animator.SetBool(AnimationString.IsOnWall, value);
            }

        }
    }

    [SerializeField]
    private bool _isOnCelling = true;
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCelling;
        }

        private set
        {
            _isOnCelling = value;

            if (animator != null)
            {
                animator.SetBool(AnimationString.IsOnCelling, value);
            }

        }
    }
    private void Awake()
    {
        touchinCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGround =   touchinCol.Cast(Vector2.down, castFilter,groundHits,GroundDIstance ) > 0;
        IsOnWall = touchinCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchinCol.Cast(Vector2.up, castFilter, cellingHits, cellingDistance) > 0;
    }
}
