using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A MonoBehaviour component for UI control over the sun position script
/// </summary>
public class SunControlUI_ : MonoBehaviour
{
    #region Public Fields
    [Tooltip("The directional light which is used as a sun")]
    public SunPosition sun;

    [Tooltip("The text to display the Date-Time information")]
    public Text dateTimeText;
    public InputField latInputfield;
    public InputField lonInputfield;
    public Slider timeOfDaySlider;
    public Slider dayOfYearSlider;
    public Button iterationButton;
    public Toggle march;
    public Toggle june;
    public Toggle september;
    public Toggle december;

    public bool useCurrentTimeLocation = true;
    public bool useSpecificDates = true;

    [Header("Iteration Settings")]
    public float startingHour = 6;
    public float endingHour = 20;
    public float minutesStep = 10.0f;
    public bool iterateDay = false;

    public DateTime startDate;
    public DateTime endDate;

    [HideInInspector]
    public DateTime dateTime;
    public DateTime this_is_now;
    #endregion

    #region Private Fields and Properties
    private float lastValue;
    private TimeZoneInfo localTimeZone;
    #endregion

    #region MonoBehaviour Methods
    void Awake()
    {
        try
        {
            localTimeZone = TimeZoneInfo.Local;

            if (useCurrentTimeLocation)
            {
                if (double.TryParse(latInputfield.text, out sun.lat) && double.TryParse(lonInputfield.text, out sun.lon))
                {
                    sun.lat = float.Parse(latInputfield.text);
                    sun.lon = float.Parse(lonInputfield.text);

                    var now = GetLocalNow();

                    timeOfDaySlider.SetValueWithoutNotify(Mathf.Floor((float)now.TimeOfDay.TotalMinutes));
                    dayOfYearSlider.SetValueWithoutNotify(Mathf.Floor(now.DayOfYear));
                    sun.SetNewDate(now);
                    this_is_now = now;
                    dateTimeText.text = now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else if (useSpecificDates)
            {
                if (latInputfield != null && double.TryParse(latInputfield.text, out sun.lat) && lonInputfield != null && double.TryParse(lonInputfield.text, out sun.lon))
                {
                    sun.lat = float.Parse(latInputfield.text);
                    sun.lon = float.Parse(lonInputfield.text);

                    var now = GetLocalNow();
                    timeOfDaySlider.value = Mathf.Floor((float)now.TimeOfDay.TotalMinutes);

                    if (march.isOn)
                        now = new DateTime(now.Year, 3, 21, now.Hour, now.Minute, now.Second);
                    else if (june.isOn)
                        now = new DateTime(now.Year, 6, 21, now.Hour, now.Minute, now.Second);
                    else if (september.isOn)
                        now = new DateTime(now.Year, 9, 23, now.Hour, now.Minute, now.Second);
                    else if (december.isOn)
                        now = new DateTime(now.Year, 12, 21, now.Hour, now.Minute, now.Second);

                    sun.SetNewDate(now);
                    this_is_now = now;
                    dateTimeText.text = now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    void Update()
    {
        if (iterateDay)
        {
            if (timeOfDaySlider.value <= endingHour * 60.0f)
            {
                timeOfDaySlider.value += minutesStep;
            }
            else
            {
                timeOfDaySlider.value = lastValue;
                ToggleIteration();
            }
        }
    }
    #endregion

    #region Public Methods

    public void OnLocationChanged(float lat, float lon)
    {
        sun.lat = lat;
        sun.lon = lon;
        sun.UpdateDateTime();
    }

    public void SetCurrentDateTime()
    {
        if (latInputfield != null && double.TryParse(latInputfield.text, out sun.lat) && lonInputfield != null && double.TryParse(lonInputfield.text, out sun.lon))
        {
            sun.lat = float.Parse(latInputfield.text);
            sun.lon = float.Parse(lonInputfield.text);

            var now = GetLocalNow();
            timeOfDaySlider.value = Mathf.Floor((float)now.TimeOfDay.TotalMinutes);
            dayOfYearSlider.value = Mathf.Floor(now.DayOfYear);
            sun.SetNewDate(now);
            this_is_now = now;
            dateTimeText.text = now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    public void SetTimeOfDay(float minutes)
    {
        var curDate = sun.date;
        var hours = Mathf.Floor(minutes / 60.0f);
        var mins = minutes - (hours * 60);
        var newDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0).AddHours(hours).AddMinutes(mins);
        sun.SetNewDate(newDate);
        sun.UpdateDateTime();
        this_is_now = newDate;
        dateTimeText.text = sun.date.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void SetDayOfYear(float day)
    {
        var curDate = sun.date;
        var newDate = new DateTime(curDate.Year, 1, 1, curDate.Hour, curDate.Minute, curDate.Second).AddDays(day - 1);
        sun.SetNewDate(newDate);
        sun.UpdateDateTime();
        this_is_now = newDate;
        dateTimeText.text = sun.date.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void SetLatitude(string lat)
    {
        sun.lat = float.Parse(lat);
        sun.UpdateDateTime();
    }

    public void SetLongitude(string lon)
    {
        sun.lon = float.Parse(lon);
        sun.UpdateDateTime();
    }

    public void ToggleIteration()
    {
        iterateDay = !iterateDay;
        if (iterateDay)
        {
            lastValue = timeOfDaySlider.value;
            iterationButton.transform.GetChild(0).GetComponent<Text>().text = "STOP";
            timeOfDaySlider.value = startingHour * 60.0f;
        }
        else
        {
            iterationButton.transform.GetChild(0).GetComponent<Text>().text = "START";
        }
    }

    public void OnMarchPressed(bool toggle)
    {
        if (toggle) SetSpecificDate(3, 21);
    }

    public void OnJunePressed(bool toggle)
    {
        if (toggle) SetSpecificDate(6, 21);
    }

    public void OnSeptemberPressed(bool toggle)
    {
        if (toggle) SetSpecificDate(9, 23);
    }

    public void OnDecemberPressed(bool toggle)
    {
        if (toggle) SetSpecificDate(12, 21);
    }

    #endregion

    #region Private Methods

    private DateTime GetLocalNow()
    {
        DateTime utcNow = DateTime.UtcNow;
        return TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
    }

    private void SetSpecificDate(int month, int day)
    {
        var curDate = sun.date;
        var hours = Mathf.Floor(timeOfDaySlider.value / 60.0f);
        var mins = timeOfDaySlider.value - (hours * 60);
        var newDate = new DateTime(curDate.Year, month, day, 0, 0, 0).AddHours(hours).AddMinutes(mins);

        sun.SetNewDate(newDate);
        sun.UpdateDateTime();
        this_is_now = newDate;
        dateTimeText.text = sun.date.ToString("yyyy-MM-dd HH:mm:ss");
    }

    #endregion
}
