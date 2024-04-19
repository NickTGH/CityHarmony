using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public float duration = 5f;
    [SerializeField]
    private float percentage;
    [SerializeField] private Gradient gradient;
    private Light2D _light;
    private float _startTime;

    [SerializeField]
    private bool raining = false;

    [SerializeField]
    private List<GameObject> weatherParticles;
    [SerializeField]
    private List<GameObject> lights;
    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _startTime = Time.time;
        percentage = 0.5f;
        StartCoroutine(ActivateRain());
    }

    // Update is called once per frame
    void Update()
    {
        var timeElapsed = Time.time - _startTime;
        percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
        percentage = Mathf.Clamp01(percentage);

        _light.color = gradient.Evaluate(percentage);
        if (percentage < 0.1f || percentage > 0.9f)
        {
            lights = GameObject.FindGameObjectsWithTag("Light").ToList();
            foreach (var light in lights)
            {
                light.GetComponent<Light2D>().enabled = true;
            }
        }
        else
        {
            lights = GameObject.FindGameObjectsWithTag("Light").ToList();
            foreach (var light in lights)
            {
                light.GetComponent<Light2D>().enabled = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (weatherParticles.Count == 0)
        {
            weatherParticles = GameObject.FindGameObjectsWithTag("Weather").ToList();
        }
    }
    private IEnumerator ActivateRain()
    {
        System.Random rnd = new System.Random();
        int timeUntillRain = rnd.Next(60,120);
        int timeRaining = rnd.Next(15,30);
        if (raining)
        {
            foreach (var particleSystem in weatherParticles)
            {
                particleSystem.GetComponentInChildren<ParticleSystem>().Play();
            }
            yield return new WaitForSeconds(timeRaining);
            raining = false;
        }
        if (!raining)
        {
            foreach (var particleSystem in weatherParticles)
            {
                particleSystem.GetComponentInChildren<ParticleSystem>().Stop();
            }
            yield return new WaitForSeconds(timeUntillRain);
            raining = true;
        }
        StartCoroutine(ActivateRain());
    }
    public float GetLightLevel()
    {
        float lightLevel = percentage * 100;
        return lightLevel;
    }
}
