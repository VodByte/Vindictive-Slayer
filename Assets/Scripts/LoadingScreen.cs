using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider slider;
    public Text text;

    private void Start()
    {
        StartCoroutine(LoadAsynchronously(2));
    }

    private IEnumerator LoadAsynchronously(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            text.text = "LOADING...[" + (progress * 100.0f).ToString() + "]%";
            yield return null;
        }
    }
}
