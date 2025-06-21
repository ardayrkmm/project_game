using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_dialog : MonoBehaviour
{
    public string[] dialogLines;   // Baris-baris dialog
    private bool playerInRange = false; // Menyimpan status apakah pemain di dalam area trigger
    public GameObject chatButton; // Tombol chat yang muncul di UI (Canvas)

    void Update()
    {
        // Jika pemain dalam area trigger dan menekan tombol
        if (playerInRange)
        {
            // Cek jika tombol chat ditekan di layar (mobile)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Cek apakah touch sedang berada di posisi tombol chat
                Vector2 touchPosition = touch.position;
                if (RectTransformUtility.RectangleContainsScreenPoint(chatButton.GetComponent<RectTransform>(), touchPosition))
                {
                    StartDialog();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Jika pemain masuk dalam area trigger NPC
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            chatButton.SetActive(true); // Tampilkan tombol chat
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Jika pemain keluar dari area trigger NPC
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            chatButton.SetActive(false); // Sembunyikan tombol chat
        }
    }

    void StartDialog()
    {
        // Mulai dialog dengan NPC
        FindObjectOfType<DialogManager>().StartDialog(dialogLines);
        chatButton.SetActive(false); // Sembunyikan tombol setelah dialog dimulai
    }
}
