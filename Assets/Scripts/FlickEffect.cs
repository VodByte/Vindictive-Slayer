using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickEffect : MonoBehaviour 
{
    public float flickRate = 0.2f;
    [Range(0.01f, 0.3f)]
    public float flickRateRandomValue = 0.2f;
    [Range(0.01f, 0.3f)]
    public float flickValue = 0.3f;

    SpriteRenderer sr;
    Color initalColor;
    float initalFlickRate;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initalColor = sr.color;
        initalFlickRate = flickRate;
        StartCoroutine(Flick());
    }

    IEnumerator Flick()
    {
        do
        {
            float h, s, v;
            Color.RGBToHSV(initalColor, out h, out s, out v);
            v += Random.Range(-0.3f, 0);
            flickRate = initalFlickRate + Random.Range(-flickRateRandomValue, flickRateRandomValue);
            sr.color = Color.HSVToRGB(h, s, v);
            yield return new WaitForSeconds(flickRate);
        } while (true);
    }
}
