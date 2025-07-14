using UnityEngine;

public class OpenDialog : MonoBehaviour
{
    public GameObject buttonDialog;     // Drag btn_dialog dari hierarki
    public GameObject canvasDialog;     // Drag Percakapan dari hierarki

    private void Start()
    {
        if (buttonDialog != null) buttonDialog.SetActive(false);
        if (canvasDialog != null) canvasDialog.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player memasuki area NPC");
            if (buttonDialog != null) buttonDialog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player meninggalkan area NPC");
            if (buttonDialog != null) buttonDialog.SetActive(false);
            if (canvasDialog != null) canvasDialog.SetActive(false);
        }
    }

    public void ShowDialog()
    {
        Debug.Log("Tombol dialog ditekan, munculkan percakapan");
        if (canvasDialog != null) canvasDialog.SetActive(true);
    }
}
