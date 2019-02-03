using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public float toggleTime = 1.0f;
    public float fadeInTime = 0.5f;
    private bool isReady = false;
    private AudioSource[] audioSource;

    private enum MenuStatus
    {
        Restart,
        ReturnTitle,
        Quit,
    }
    MenuStatus currentMenu = MenuStatus.Restart;
    MenuStatus prevMenu = MenuStatus.Restart;

    private Color normalCol;
    private Color hightLightCol;

    IEnumerator currentSelectCoroutin;
    IEnumerator currentCancleCoroutin;

    private void Start()
    {
        audioSource = GetComponents<AudioSource>();
        GameObject child = transform.GetChild(0).gameObject;
        normalCol = child.GetComponent<SpriteRenderer>().color;

        GameObject sampleChild = transform.GetChild(transform.childCount - 1).gameObject;
        hightLightCol = sampleChild.GetComponent<SpriteRenderer>().color;

        child.GetComponent<SpriteRenderer>().color = hightLightCol;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        SpriteRenderer[] sr = new SpriteRenderer[3];
        for (int i = 0; i < 3; i++)
        {
            sr[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
            sr[i].color = new Color(sr[i].color.r, sr[i].color.g, sr[i].color.b, 0.0f);
        }

        do
        {
            foreach (var i in sr)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + 1.0f / fadeInTime * Time.deltaTime);
            }

            yield return null;
        } while (sr[0].color.a < 1.0f || sr[1].color.a < 1.0f || sr[0].color.a < 1.0f);

        isReady = true;
    }

    private void Update()
    {
        if (!isReady) return;

        // 選択肢の数値移動
        if (Input.GetKeyDown(KeyCode.W))
        {
            prevMenu = currentMenu;
            --currentMenu;
            if ((int)currentMenu < 0) currentMenu = MenuStatus.Quit;
            ChangeColor();

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            prevMenu = currentMenu;
            ++currentMenu;
            if ((int)currentMenu > 2) currentMenu = MenuStatus.Restart;
            ChangeColor();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource[0].Play();
            switch (currentMenu)
            {
                case MenuStatus.Restart:
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
                case MenuStatus.ReturnTitle:
                    SceneManager.LoadScene(0);
                    break;
                case MenuStatus.Quit:
                    Debug.Log("Quit Game");
                    Application.Quit();
                    break;
            }
        }
    }

    /// <summary>
    /// Changes the color.
    /// </summary>
    private void ChangeColor()
    {
        // 行先をつける
        if (currentSelectCoroutin != null)
        {
            StopCoroutine(currentSelectCoroutin);
            for (int i = 0; i < 3; i++)
            {
                if (i != (int)currentMenu)
                {
                    transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = normalCol;
                }
            }
        }
        currentSelectCoroutin = HandleMenuColor(currentMenu, true);
        StartCoroutine(currentSelectCoroutin);

        // 元の選択肢を消す
        if (currentCancleCoroutin != null)
        {
            StopCoroutine(currentCancleCoroutin);
        }
        currentCancleCoroutin = HandleMenuColor(prevMenu, false);
        StartCoroutine(currentCancleCoroutin);
    }

    /// <summary>
    /// Handles the color of the menu.
    /// </summary>
    /// <returns>The menu color.</returns>
    /// <param name="index">Index.</param>
    /// <param name="isIn">If set to <c>true</c> is in.</param>
    private IEnumerator HandleMenuColor(MenuStatus index, bool isIn)
    {
        float timer = 0.0f;

        // 二つTextの情報を格納する
        SpriteRenderer SpriteRenderer = transform.GetChild((int)index).gameObject.GetComponent<SpriteRenderer>();
        Color targetCol, prevCol;

        // isIn選択されたない状態の色を切り替え
        if (isIn)
        {

            prevCol = SpriteRenderer.color = normalCol;
            targetCol = hightLightCol;

        }
        else
        {
            prevCol = SpriteRenderer.color = hightLightCol;
            targetCol = normalCol;
        }

        do
        {
            timer += Time.deltaTime;

            float menuR = prevCol.r * (toggleTime - timer) + targetCol.r * (1.0f + toggleTime - timer);
            float menuG = prevCol.g * (toggleTime - timer) + targetCol.g * (1.0f + toggleTime - timer);
            float menuB = prevCol.b * (toggleTime - timer) + targetCol.b * (1.0f + toggleTime - timer);
            float menuA = prevCol.a * (toggleTime - timer) + targetCol.a * (1.0f + toggleTime - timer);
            SpriteRenderer.color = new Color(menuR, menuG, menuB, menuA);

            yield return null;
        } while (timer < toggleTime);
    }
}