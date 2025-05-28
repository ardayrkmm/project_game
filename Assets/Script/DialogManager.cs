using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    private string[] dialogLines;
    private int currentLine;
    private bool isActive;

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Q))
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

    public void StartDialog(string[] lines)
    {
        dialogLines = lines;
        currentLine = 0;
        dialogText.text = dialogLines[currentLine];
        dialogPanel.SetActive(true);
        isActive = true;
    }

    public void EndDialog()
    {
        dialogPanel.SetActive(false);
        isActive = false;
    }
}
