using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float toggleTime = 1.0f;

    private enum MenuStatus
    {
        Start,
        Option,
        Quit,
    }
    MenuStatus currentMenu = MenuStatus.Start;
    MenuStatus prevMenu = MenuStatus.Start;

    private Color normalMenuCol;
    private Color normalTextCol;

    private Color hightLightMenuCol;
    private Color hightLightTextCol;

    IEnumerator currentSelectCoroutin;
    IEnumerator currentCancleCoroutin;

    private void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        normalMenuCol = child.GetComponent<SpriteRenderer>().color;
        normalTextCol = child.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

        GameObject sampleChild = transform.GetChild(transform.childCount - 1).gameObject;
        hightLightMenuCol = sampleChild.GetComponent<SpriteRenderer>().color;
        hightLightTextCol = sampleChild.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

        child.GetComponent<SpriteRenderer>().color = hightLightMenuCol;
        child.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = hightLightTextCol;

    }

    private void Update()
    {
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
            if ((int)currentMenu > 2) currentMenu = MenuStatus.Start;
            ChangeColor();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentMenu)
            {
                case MenuStatus.Start:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
                case MenuStatus.Option:
                    Debug.Log("WIP");
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
                    transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = normalMenuCol;
                    transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = normalTextCol;
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

        // 二つSpriteRendererの情報を格納する
        GameObject menu = transform.GetChild((int)index).gameObject;
        SpriteRenderer menuSr = menu.GetComponent<SpriteRenderer>();
        SpriteRenderer textSr = menu.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Color targetMenuCol, targetTextCol, prevMenuCol, prevTextCol;

        // isIn選択されたない状態の色を切り替え
        if (isIn)
        {

            prevMenuCol = menuSr.color = normalMenuCol;
            prevTextCol = textSr.color = normalTextCol;
            targetMenuCol = hightLightMenuCol;
            targetTextCol = hightLightTextCol;

        }
        else
        {
            prevMenuCol = menuSr.color = hightLightMenuCol;
            prevTextCol = textSr.color = hightLightTextCol;
            targetMenuCol = normalMenuCol;
            targetTextCol = normalTextCol;
        }

        //menuSr.color = targetMenuCol;
        //textSr.color = targetTextCol;
        //yield return null;

        do
        {
            timer += Time.deltaTime;

            float menuR = prevMenuCol.r * (toggleTime - timer) + targetMenuCol.r * (1.0f + toggleTime - timer);
            float menuG = prevMenuCol.g * (toggleTime - timer) + targetMenuCol.g * (1.0f + toggleTime - timer);
            float menuB = prevMenuCol.b * (toggleTime - timer) + targetMenuCol.b * (1.0f + toggleTime - timer);
            menuSr.color = new Color(menuR, menuG, menuB);

            float textR = prevTextCol.r * (toggleTime - timer) + targetTextCol.r * (1.0f + toggleTime - timer);
            float textG = prevTextCol.g * (toggleTime - timer) + targetTextCol.g * (1.0f + toggleTime - timer);
            float textB = prevTextCol.b * (toggleTime - timer) + targetTextCol.b * (1.0f + toggleTime - timer);
            textSr.color = new Color(textR, textG, textB);

            yield return null;
        } while (timer < toggleTime);
    }
}