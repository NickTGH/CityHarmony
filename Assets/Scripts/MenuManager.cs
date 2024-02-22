using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField]
    private GameObject pauseMenu;


    [SerializeField]
    private AudioManager audioManager;

	private void Start()
	{
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.PlayGameTheme();
	}
	public void LoadScene(int sceneIndex)
    {
        audioManager.PlaySelectButtonSfx();
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
			audioManager.PlayReturnToMenuSfx();
			audioManager.PauseGameTheme();
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            audioManager.PlayReturnToMenuSfx(); 
            audioManager.PauseGameTheme();
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        audioManager.PlayReturnToMenuSfx();
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        audioManager.PlaySelectButtonSfx();
        Application.Quit();
    }
}
