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

            Vector2 deliveryKnockBack = transform.parent.localScale.x > 0 ? knockBAck : new Vector2(-knockBAck.x, knockBAck.y);
          bool gotHit  = damageable.Hit(attackDamage, deliveryKnockBack);

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
