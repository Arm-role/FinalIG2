using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Light Sun;

    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [SerializeField] private float SunRotateSpeed;

    [SerializeField] private Gradient skycolor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient groundedColor;
    [SerializeField] private Gradient FogColor;
    [SerializeField] private Gradient sunColor;

    private void Update()
    {
        TimeOfDay += Time.deltaTime * SunRotateSpeed;
        if (TimeOfDay > 24)
        {
            TimeOfDay = 0;
            ChackFarm.instance.FindOP();
        }else
        {
            UpdateSunRotation();
            UpdateLighting();
        }
    }
    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLighting();
    }
    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, TimeOfDay / 24);
        Sun.transform.rotation = Quaternion.Euler(sunRotation, Sun.transform.rotation.y, Sun.transform.rotation.z);
    }

    private void UpdateLighting()
    {
        float timeFactor = TimeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFactor);
        RenderSettings.ambientSkyColor = skycolor.Evaluate(timeFactor);
        RenderSettings.ambientGroundColor = groundedColor.Evaluate(timeFactor);
        RenderSettings.fogColor = FogColor.Evaluate(timeFactor);
        Sun.color = sunColor.Evaluate(timeFactor);
    }
}
