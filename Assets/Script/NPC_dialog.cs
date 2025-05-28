using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_dialog : MonoBehaviour
{
    public string[] dialogLines;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<DialogManager>().StartDialog(dialogLines);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
