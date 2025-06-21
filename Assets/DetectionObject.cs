using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionObject : MonoBehaviour
{
    // Start is called before the first frame update
    Collider2D col;
    public List<Collider2D> detectedCollider = new List<Collider2D>();

    private void Awake()
    {
        col = GetComponent<Collider2D>();

    }
    private void OnTriggerEnter2D(Collider2D ss)
    {
        detectedCollider.Add(ss);
    }

    private void OnTriggerExit2D(Collider2D u)
    {
        detectedCollider.Remove(u);
    }
}
