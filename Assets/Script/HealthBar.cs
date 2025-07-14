using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthSlider;
    public TMP_Text healthBar;
    DamageAble playerAble;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.Log("tidak ada player");
        }
        playerAble = GetComponent<DamageAble>();
    }
    void Start()
    {
      
       
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }
    private void OnEnable()
    {
        playerAble.healthChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        playerAble.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAble != null)
        {
            healthSlider.value = CalculateSliderPercentage(playerAble.Health, playerAble.MaxHealth);
            healthBar.text = "Darah " + playerAble.Health + " / " + playerAble.MaxHealth;
        }
    }

    private void OnPlayerHealthChanged(int newHealth, int newMaxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, newMaxHealth);
        healthBar.text = "Darah " + newHealth + " / " + newMaxHealth;
    }
}
