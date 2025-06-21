using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;  // Panel yang menampilkan dialog
    public TMP_Text dialogText;     // Tempat menampilkan teks dialog
    private string[] dialogLines;   // Baris-baris dialog
    private int currentLine;        // Indeks baris dialog yang sedang ditampilkan
    private bool isActive;          // Status dialog apakah aktif atau tidak

    void Update()
    {
        if (isActive)
        {
            // Menggunakan sentuhan untuk perangkat mobile
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    currentLine++;
                    if (currentLine < dialogLines.Length)
                    {
                        dialogText.text = dialogLines[currentLine];
                    }
                    else
                    {
                        EndDialog();
                    }
                }
            }
        }
    }

    // Memulai dialog
    public void StartDialog(string[] lines)
    {
        dialogLines = lines;
        currentLine = 0;
        dialogText.text = dialogLines[currentLine];
        dialogPanel.SetActive(true);  // Menampilkan panel dialog
        isActive = true;
    }

    // Mengakhiri dialog
    public void EndDialog()
    {
        dialogPanel.SetActive(false);  // Menyembunyikan panel dialog setelah selesai
        isActive = false;
    }
}
