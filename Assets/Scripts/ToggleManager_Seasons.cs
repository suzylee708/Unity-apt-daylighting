using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleManager_Seasons : MonoBehaviour
{
    public Button springButton;
    public Button summerButton;
    public Button fallButton;
    public Button winterButton;
    public SunPosition sunPosition;

    void Start()
    {
        springButton.onClick.AddListener(() => SetSeason(3, 21));  // March 21st
        summerButton.onClick.AddListener(() => SetSeason(6, 21));  // June 21st
        fallButton.onClick.AddListener(() => SetSeason(9, 21));    // September 21st
        winterButton.onClick.AddListener(() => SetSeason(12, 21)); // December 21st
    }

    void SetSeason(int month, int day)
    {
        sunPosition.SetNewDate(new DateTime(2024, month, day, 12, 0, 0));
    }
}
