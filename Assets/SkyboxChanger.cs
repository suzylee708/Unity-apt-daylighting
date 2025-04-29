using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material daySkybox;  // Assign in Inspector
    public Material nightSkybox; // Assign in Inspector
    public Light sunLight; // Assign your Directional Light

    public float transitionHour = 18f; // Change to night at 6 PM
    public float morningHour = 6f; // Change to day at 6 AM

    private SunPosition sunPosition; // Reference to SunPosition script

    void Start()
    {
        sunPosition = FindObjectOfType<SunPosition>();
        UpdateSkybox();
    }

    void Update()
    {
        UpdateSkybox();
    }

    void UpdateSkybox()
    {
        if (sunPosition != null)
        {
            float currentHour = sunPosition.hour; // Get the hour from SunPosition.cs

            if (currentHour >= transitionHour || currentHour < morningHour)
            {
                RenderSettings.skybox = nightSkybox;
                sunLight.intensity = 0.1f; // Dim the light at night
            }
            else
            {
                RenderSettings.skybox = daySkybox;
                sunLight.intensity = 1f; // Full brightness during the day
            }
        }
    }
}
