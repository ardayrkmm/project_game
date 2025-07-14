using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Chest : MonoBehaviour
{
    public Animator animator;
    public Button openButton;
    public TMP_Text messageText;
    [TextArea]
    public string pesanKePlayer = "Kamu mendapatkan 100 gold!";
    public float durasiTampil = 2f;

    public GameObject chestVisual;  // GameObject visual chest (sprite+animator)
    public GameObject pintu;        // Objek pintu yang akan dimunculkan

    private bool isPlayerNearby = false;
    private bool isOpened = false;

    void Start()
    {
        openButton.gameObject.SetActive(false);

        if (messageText != null)
            messageText.gameObject.SetActive(false);

        if (pintu != null)
            pintu.SetActive(false); // pintu disembunyikan di awal

        openButton.onClick.AddListener(OpenChest);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            isPlayerNearby = true;
            openButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            openButton.gameObject.SetActive(false);
        }
    }

    public void OpenChest()
    {
        if (isPlayerNearby && !isOpened)
        {
            animator.SetTrigger("Open");
            isOpened = true;
            openButton.gameObject.SetActive(false);
            ShowMessage();
            Invoke(nameof(CloseAndSpawnDoor), durasiTampil);
        }
    }

    void ShowMessage()
    {
        if (messageText != null)
        {
            messageText.text = pesanKePlayer;
            messageText.gameObject.SetActive(true);
            Invoke(nameof(HideMessage), durasiTampil);
        }
    }

    void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    void CloseAndSpawnDoor()
    {
        animator.SetTrigger("Close");

        // Delay dikit agar animasi close selesai
        Invoke(nameof(FinishChest), 1f);
    }

    void FinishChest()
    {
        if (chestVisual != null)
            chestVisual.SetActive(false); // Sembunyikan chest

        if (pintu != null)
            pintu.SetActive(true);        // Munculkan pintu
    }

}
