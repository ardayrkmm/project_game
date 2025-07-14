using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    Collider2D col;

    public LayerMask detectionLayer; // tambahkan di inspector

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collec)
    {
        // Hanya tambahkan jika layer target ada di detectionLayer
        if (((1 << collec.gameObject.layer) & detectionLayer) != 0)
        {
            detectedColliders.Add(collec);
        }
    }

    private void OnTriggerExit2D(Collider2D collec)
    {
        detectedColliders.Remove(collec);
    }

}

