using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DamageAble : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityEvent<int, Vector2> damagebleHit;
    public UnityEvent<int, int> healthChanged;

    [SerializeField]
    private int _maxHealth = 100;

    Animator animator;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }

        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public void Awake()
    {
        animator = GetComponent<Animator> ();
    }

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
    public Image[] heartImages;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged.Invoke (_health, MaxHealth);
            if(_health <= 0)
            {
                IsAlive = false;
             
            }
            
        }
    }
    private void DisableEnemy()
    {
        // Menonaktifkan objek musuh ketika kesehatan 0
        gameObject.SetActive(false);
    }

    [SerializeField]
    private bool _isAlive = true;

    private bool isInvicible= false;
    private float timeSinceHit =0;
    public float inivicibilityTime = 0.25f;


    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }

        set
        {
            _isAlive = value;
            animator.SetBool(AnimationString.IsAlive, value);
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvicible)
        {
            if(timeSinceHit > inivicibilityTime)
            {
                isInvicible = false;
                timeSinceHit = 0;
                
            }

            timeSinceHit += Time.deltaTime;
        }   
    }

    public bool Hit(int Damage, Vector2 Knock)
    {
        if (_isAlive && !isInvicible)
        {
            Health -= Damage;
            isInvicible = true;
         
            animator.SetTrigger(AnimationString.hitTrigger);
            LockVelocity = true;
            damagebleHit.Invoke(Damage, Knock);
            return true;
        }

        return false;
    }
    private void UpdateHealthBar()
    {
        int heartsToShow = Mathf.CeilToInt((float)_health / _maxHealth * heartImages.Length);

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < heartsToShow)
                heartImages[i].enabled = true;  // Menampilkan gambar hati
            else
                heartImages[i].enabled = false; // Menyembunyikan gambar hati
        }
    }
}
