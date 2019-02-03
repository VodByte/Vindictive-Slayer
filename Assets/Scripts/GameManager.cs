#define Debug_On
//---------------------------------------------------------------------
// GameManger.cs
// ゲーム流れの管理
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 始めの日
//---------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //------------------------------------------
    //変数宣言
    //------------------------------------------
    CharaManager cm;
    public float playerInSpeed = 1.0f;
    public Vector2 playerInPos = new Vector2(-5.741f, -2.632f);
    public Transform leftCreatePoint;
    public Transform RightCreatePoint;
    public GameObject pauseMenu;
    public Text scoreBoard;

    [Header("Game Over")]
    public GameObject GameOverMenu;
    public GameObject GameOverMask;
    public float maskMoveSpeed = 1.0f;
    public Transform GameOverText_UP;
    public Transform GameOverText_DOWN;
    public float TextMoveSpeed = 1.0f;
    public SpriteRenderer blackScreen;
    public float endAplha = 0.5f;
    public float blackFadeTime = 2.0f;

    private const float StageLength = 20.0f;       // ステージの長さ(秒)
    private TimeStamp headTimeStamp;
    public static bool isStageClear = false;
    //-------------------------------------------------
    // 初期化処理
    //-------------------------------------------------
    private void Awake()
    {
        // 敵を配置する。このやり方は雑なので、後は別の方法でやる
        cm = GetComponent<CharaManager>();
        cm.CreateChara(new Vector2(leftCreatePoint.position.x, -2.632f), CharaManager.CharaType.Player);
    }

    private void Start()
    {
        // 敵を配置する
        headTimeStamp = LoadCSV.timeStampQueue.Dequeue();
        StartCoroutine(CreateEnemy());
        StartCoroutine(MovePlayerIn());
    }

    IEnumerator MovePlayerIn()
    {
        do
        {
            yield return null;
            GameInfo.PlayerInfo.gameObject.transform.Translate(Vector2.right * playerInSpeed * Time.deltaTime, Space.World);
        } while (GameInfo.PlayerInfo.pos.x < playerInPos.x);
        GetComponent<InputManager>().enabled = true;
    }

    //-------------------------------------------------
    // 更新処理(フレームごとに行う)
    //-------------------------------------------------
    private void Update()
    {
        // Stage Clearを判定
        if (Time.timeSinceLevelLoad >= StageLength && GameInfo.PlayerInfo.iHp > 0)
        {
            isStageClear = true;
        }

        // プレイヤのScoreを表示する
        scoreBoard.text = "Score:" + GameInfo.total_Score;

        // Game Over
        if (GameInfo.PlayerInfo.iHp <= 0)
        {
            // Light Black Screen
            if (blackScreen.color.a < endAplha)
            {
                Color prevColor = blackScreen.color;
                blackScreen.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a + endAplha / blackFadeTime * Time.deltaTime);
            }
            else
            {
                GameOverMenu.SetActive(true);

                // Move Mask
                GameOverMask.transform.position = Vector2.Lerp(GameOverMask.transform.position, Vector2.zero, maskMoveSpeed * Time.deltaTime);

                // Move Text
                GameOverText_UP.localPosition = Vector2.Lerp(GameOverText_UP.localPosition, Vector2.zero, TextMoveSpeed * Time.deltaTime);
                GameOverText_DOWN.localPosition = Vector2.Lerp(GameOverText_DOWN.localPosition, Vector2.zero, TextMoveSpeed * Time.deltaTime);
            }
        }

        // Game Clear
        if (isStageClear)
        {
            // Light Black Screen
            if (blackScreen.color.a < endAplha)
            {
                Color prevColor = blackScreen.color;
                blackScreen.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a + endAplha / blackFadeTime * Time.deltaTime);
            }
            else
            {
                GetComponent<InputManager>().enabled = false;
                GameInfo.PlayerInfo.gameObject.transform.Translate(Vector2.right * playerInSpeed * Time.deltaTime);

                // fade out bgm
                GetComponent<AudioSource>().volume -= 0.2f * Time.deltaTime;
            }

            if (GameInfo.PlayerInfo.pos.x > RightCreatePoint.position.x + 2.0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

#if Debug_On
        // R を押したら、Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
#endif

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

	private IEnumerator CreateEnemy()
	{
		do
		{
			if (Time.timeSinceLevelLoad >= headTimeStamp.time)
			{
				Vector2 createPoint;
				if (headTimeStamp.createDir == 0)
				{
					createPoint = leftCreatePoint.position;
				}
				else
				{
					createPoint = RightCreatePoint.position;
				}

				if(headTimeStamp.enemyType == 1) createPoint = new Vector2(createPoint.x, -2.3f);

				cm.CreateChara(	createPoint, (CharaManager.CharaType)(headTimeStamp.enemyType + 1));
				if (LoadCSV.timeStampQueue.Count != 0) headTimeStamp = LoadCSV.timeStampQueue.Dequeue();
			}

			yield return null;
		} while (LoadCSV.timeStampQueue.Count != 0);

	}
}