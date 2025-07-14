using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DialogLine
{
    [TextArea(2, 5)]
    public string message;
    public bool isLeftSide;
}

public class DialogManager1 : MonoBehaviour
{
    public GameObject leftDialog, rightDialog;
    public TMP_Text leftDialogText, rightDialogText;

    public Button leftNextButton;
    public Button rightNextButton;

    public DialogLine[] dialogLines;

    private int currentLine = 0;

    void Start()
    {
        Time.timeScale = 0f;

        // Pasang listener untuk kedua tombol
        leftNextButton.onClick.AddListener(NextLine);
        rightNextButton.onClick.AddListener(NextLine);

        ShowLine();
    }

    void ShowLine()
    {
        leftDialog.SetActive(false);
        rightDialog.SetActive(false);
        leftNextButton.gameObject.SetActive(false);
        rightNextButton.gameObject.SetActive(false);

        var line = dialogLines[currentLine];

        if (line.isLeftSide)
        {
            leftDialog.SetActive(true);
            leftDialogText.text = line.message;
            leftNextButton.gameObject.SetActive(true);
            rightDialogText.text = "";
        }
        else
        {
            rightDialog.SetActive(true);
            rightDialogText.text = line.message;
            rightNextButton.gameObject.SetActive(true);
            leftDialogText.text = "";
        }
    }

    public void NextLine()
    {
        currentLine++;
        if (currentLine < dialogLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        leftDialog.SetActive(false);
        rightDialog.SetActive(false);
        leftNextButton.gameObject.SetActive(false);
        rightNextButton.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
