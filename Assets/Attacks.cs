using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    // Start is called before the first frame update
    public int attackDamage = 10;
    public Vector2 knockBAck = Vector2.zero;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Something entered: " + col.name); // <-- Tambahan
        DamageAble damageable = col.GetComponent<DamageAble>();
        if (damageable != null)
        {
          bool gotHit  = damageable.Hit(attackDamage, knockBAck);

            if (gotHit)
            {
                Debug.Log(col.name + ": " + attackDamage);
            }
          
        }
        else
        {
            Debug.Log(col.name + " has no DamageAble component.");
        }
    }
}
