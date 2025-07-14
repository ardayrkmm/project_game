using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Simpan : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // Ganti sesuai nama scene utama kamu

    public void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", currentScene);
        PlayerPrefs.Save();
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("LastScene"))
        {
            string lastScene = PlayerPrefs.GetString("LastScene");
            Debug.Log("Melanjutkan ke scene: " + lastScene);

            Time.timeScale = 1f; // Penting: pastikan game tidak ter-pause

            if (Application.CanStreamedLevelBeLoaded(lastScene))
            {
                SceneManager.LoadScene(lastScene);
            }
            else
            {
                Debug.LogError("Scene tidak ditemukan: " + lastScene);
            }
        }
        else
        {
            Debug.Log("Belum ada data permainan sebelumnya.");
        }
    }

    public void SaveAndExit()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastScene", currentScene);
        PlayerPrefs.Save();

        Time.timeScale = 1f; // Pastikan game tidak freeze saat kembali ke menu

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Keluar dari game...");



        Application.Quit();

    }

}
