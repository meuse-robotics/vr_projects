using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [Header("Sun")]
    [SerializeField] Light sun;
    [SerializeField] Gradient sunColor;
    [SerializeField] AnimationCurve sunIntensity;

    [Header("Environment")]
    [SerializeField] AnimationCurve lightingIntensityMultiplier;
    [SerializeField] AnimationCurve reflectionsIntensityMultiplier;

    public void sunControl(float tim)
    {
        // 太陽の角度
        float rotX;
        float rotY;
        if (tim < 0.5f)
            rotX = tim * 90f;
        else
            rotX = (1f - tim) * 90f;
        rotY = -90f + 180f * tim;
        sun.transform.rotation = Quaternion.Euler(rotX, rotY, 0f);

        // 太陽の明るさ
        sun.intensity = sunIntensity.Evaluate(tim) * 2;
        if (sun.intensity <= 0.2 && sun.gameObject.activeInHierarchy)
            sun.gameObject.SetActive(false);
        else if (sun.intensity > 0.2 && !sun.gameObject.activeInHierarchy)
            sun.gameObject.SetActive(true);
        
        // 太陽の色
        sun.color = sunColor.Evaluate(tim);

        // 周囲の明るさ
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(tim);

        // 反射光
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(tim);
    }
}
