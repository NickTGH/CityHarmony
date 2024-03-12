using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public float duration = 5f;

    [SerializeField] private Gradient gradient;
    private Light2D _light;
    private float _startTime;

    [SerializeField]
    private List<GameObject> lights;
    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the time elapsed since the start time
        var timeElapsed = Time.time - _startTime;
        // Calculate the percentage based on the sine of the time elapsed
        var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
        // Clamp the percentage to be between 0 and 1
        percentage = Mathf.Clamp01(percentage);

        _light.color = gradient.Evaluate(percentage);
        if (percentage < 0.2f || percentage > 0.8f)
        {
            //Turn on lights if they are off
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
}
