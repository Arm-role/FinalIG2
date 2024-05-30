using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Light Sun;

    [SerializeField, Range(0, 60)] private float TimeOfDay;

    [SerializeField] private float SunRotateSpeed;

    [SerializeField] private Gradient skycolor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient groundedColor;
    [SerializeField] private Gradient FogColor;
    [SerializeField] private Gradient sunColor;

    private float totaltime = 60;
    const float MaxTime = 60;
    private void Update()
    {
        TimeOfDay += Time.deltaTime * SunRotateSpeed;
        totaltime -= Time.deltaTime * SunRotateSpeed;

        SpawnRaid.instance.TimeWorld(totaltime);

        if (TimeOfDay > MaxTime)
        {
            TimeOfDay = 0;
            totaltime = MaxTime;
            StartCoroutine(waithForRaid());
        }
        else
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
        float sunRotation = Mathf.Lerp(-90, 270, TimeOfDay / 300);
        Sun.transform.rotation = Quaternion.Euler(sunRotation, Sun.transform.rotation.y, Sun.transform.rotation.z);
    }

    private void UpdateLighting()
    {
        float timeFactor = TimeOfDay / MaxTime;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFactor);
        RenderSettings.ambientSkyColor = skycolor.Evaluate(timeFactor);
        RenderSettings.ambientGroundColor = groundedColor.Evaluate(timeFactor);
        RenderSettings.fogColor = FogColor.Evaluate(timeFactor);
        Sun.color = sunColor.Evaluate(timeFactor);
    }

    IEnumerator waithForRaid()
    {
        bool isPass = true;
        Debug.Log("ON");
        yield return new WaitForSeconds(10);
        Debug.Log("OFF");
        if (isPass)
        {
            ChackFarm.instance.FindOP();
            isPass = false;
        }
    }
}
