using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource theme;

    [SerializeField]
    private AudioSource selectButtonSfx;

    [SerializeField]
    private AudioSource activateCameraSfx;

    [SerializeField]
    private AudioSource destroySfx;

    [SerializeField]
    private AudioSource placeSfx;

    [SerializeField]
    private AudioSource returnToMenuSfx;

    [SerializeField]
    private AudioSource enterStateSfx;

    [SerializeField]
    private AudioSource failedToRemoveSfx;

	public void Awake()
	{
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("Audio");
        if (musicObjects.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
	}

    public void PlayGameTheme()
    {
        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            theme.Play();
        }
        else
        {
            theme.Stop();
        }
    }
    public void PauseGameTheme()
    {
        theme.Pause();
    }

	public void PlaySelectButtonSfx()
	{
		selectButtonSfx.Play();
	}
	public void PlayActivateCameraSfx()
	{
		activateCameraSfx.Play();
	}
	public void PlayDestroySfx()
	{
		destroySfx.Play();
	}
	public void PlayPlaceSfx()
	{
		placeSfx.Play();
	}
	public void PlayReturnToMenuSfx()
	{
		returnToMenuSfx.Play();
	}
	public void PlayEnterStateSfx()
	{
		enterStateSfx.Play();
	}
	public void PlayFailedToRemoveSfx()
	{
		failedToRemoveSfx.Play();
	}
}
