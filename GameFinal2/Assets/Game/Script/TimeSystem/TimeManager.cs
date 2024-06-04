using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Light Sun;

    [SerializeField, Range(0, 300)] private float TimeOfDay;

    [SerializeField] private float SunRotateSpeed;
    [SerializeField] private float RaidCoolDown;

    [SerializeField] private Gradient skycolor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient groundedColor;
    [SerializeField] private Gradient FogColor;
    [SerializeField] private Gradient sunColor;

    private float totaltime = 300;
    const float MaxTime = 300;
    bool isRaid = true;

    int CountSpawn = 0;
    private void Update()
    {
        TimeOfDay += Time.deltaTime * SunRotateSpeed;
        totaltime -= Time.deltaTime * SunRotateSpeed;

        SpawnRaid.instance.TimeWorld(totaltime);

        if (TimeOfDay > MaxTime)
        {
            TimeOfDay = 0;
            totaltime = MaxTime;
            //CountSpawn;
        }
        else
        {
            UpdateSunRotation();
            UpdateLighting();
        }
        if(CountSpawn > 0)
        {
            if (isRaid)
            {
                SpawnRaid.instance.Raid();
                StartCoroutine(waithForRaid());
            }
        }
        
    }
    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLighting();
    }
    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, TimeOfDay / MaxTime);
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
        isRaid = false;
        CountSpawn--;
        yield return new WaitForSeconds(RaidCoolDown);
        isRaid = true;
    }
}
