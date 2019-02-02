using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public float toggleTime = 1.0f;

    private enum MenuStatus
    {
        Resume,
        Restart,
        Quit,
    }
    MenuStatus currentMenu = MenuStatus.Resume;
    MenuStatus prevMenu = MenuStatus.Resume;

    private Color normalCol;
    private Color hightLightCol;

    IEnumerator currentSelectCoroutin;
    IEnumerator currentCancleCoroutin;

    private void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        normalCol = child.GetComponent<Text>().color;

        GameObject sampleChild = transform.GetChild(transform.childCount - 2).gameObject;
        hightLightCol = sampleChild.GetComponent<Text>().color;

        child.GetComponent<Text>().color = hightLightCol;
    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
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
            if ((int)currentMenu > 2) currentMenu = MenuStatus.Resume;
            ChangeColor();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentMenu)
            {
                case MenuStatus.Resume:
                    Time.timeScale = 1.0f;
                    gameObject.SetActive(false);
                    break;
                case MenuStatus.Restart:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                    transform.GetChild(i).gameObject.GetComponent<Text>().color = normalCol;
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
        Text text = transform.GetChild((int)index).gameObject.GetComponent<Text>();
        Color targetCol, prevCol;
        
        // isIn選択されたない状態の色を切り替え
        if (isIn)
        {

            prevCol = text.color = normalCol;
            targetCol = hightLightCol;

        }
        else
        {
            prevCol = text.color = hightLightCol;
            targetCol = normalCol;
        }

        do
        {
            timer += Time.fixedDeltaTime;

            float menuR = prevCol.r * (toggleTime - timer) + targetCol.r * (1.0f + toggleTime - timer);
            float menuG = prevCol.g * (toggleTime - timer) + targetCol.g * (1.0f + toggleTime - timer);
            float menuB = prevCol.b * (toggleTime - timer) + targetCol.b * (1.0f + toggleTime - timer);
            float menuA = prevCol.a * (toggleTime - timer) + targetCol.a * (1.0f + toggleTime - timer);
            text.color = new Color(menuR, menuG, menuB, menuA);

            yield return null;
        } while (timer < toggleTime);
    }
}