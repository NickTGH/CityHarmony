using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public AudioMixer audioMixer;
	Resolution[] resolutions;

	public TMP_Dropdown resolutionDropdown;
	public TMP_Dropdown graphicsDropdown;
	public Toggle fullscreenToggle;
	public Slider masterVolumeSlider;
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;
	private void Start()
	{
		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + "x" + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.width &&
				resolutions[i].height == Screen.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();

		fullscreenToggle.isOn = StaticValues.IsFullscreen;

		graphicsDropdown.value = StaticValues.QualityIndex;
		graphicsDropdown.RefreshShownValue();

		masterVolumeSlider.value = StaticValues.VolumeLevel;
		musicVolumeSlider.value = StaticValues.MusicLevel;
		sfxVolumeSlider.value = StaticValues.SfxLevel;
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
	}
	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("masterVolume", Mathf.Log10(volume)*20);
		StaticValues.VolumeLevel = volume;
	}
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
		StaticValues.MusicLevel = volume;
    }
    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
		StaticValues.SfxLevel = volume;
    }
    public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
		StaticValues.QualityIndex = qualityIndex;
	}
	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
		StaticValues.IsFullscreen = isFullscreen;
	}
}
