using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    Collider2D col;
   private void Awake()
    {
        col = GetComponent<Collider2D> ();
    }
    
    private void OnTriggerEnter2D(Collider2D collec)
    {
        detectedColliders.Add(collec);

    }
    private void OnTriggerExit2D(Collider2D collec)
    {
        detectedColliders.Remove(collec);

    }

}

