using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource exitToMenuSfx;
    private bool isPaused = false;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource selectButton;
    public void LoadScene(int sceneIndex)
    {
        selectButton.Play();
        SceneManager.LoadScene(sceneIndex);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseScene()
    {
        if (!isPaused)
        {
			exitToMenuSfx.Play();
			music.Pause();
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
			exitToMenuSfx.Play();
			music.Play();
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
