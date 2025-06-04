using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MunculInfo : MonoBehaviour
{
    public GameObject infoText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            infoText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            infoText.SetActive(false);
        }
    }
}
