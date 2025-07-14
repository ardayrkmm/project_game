using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PintuTrigger : MonoBehaviour
{
     public int requiredPoints = 5; // Berapa poin dibutuhkan agar pintu aktif
    public string nextSceneName;   // Nama Scene tujuan

    private bool isUnlocked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCon player = other.GetComponent<PlayerCon>();
            if (player != null)
            {
                if (player.GetPoints() >= requiredPoints)
                {
                    if (!isUnlocked)
                    {
                        Debug.Log("Pintu terbuka! Pindah ke scene berikutnya...");
                        isUnlocked = true;
                        SceneManager.LoadScene(nextSceneName);
                    }
                }
                else
                {
                    Debug.Log("Belum cukup poin. Kamu butuh: " + requiredPoints);
                }
            }
        }
    }
}
