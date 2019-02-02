#define Debug_On
//---------------------------------------------------------------------
// GameManger.cs
// ゲーム流れの管理
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 始めの日
//---------------------------------------------------------------------
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
    [SerializeField] Transform mark;
    [SerializeField] int[] types;
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

    //-------------------------------------------------
    // 初期化処理
    //-------------------------------------------------
    private void Awake()
    {
        // 敵を配置する。このやり方は雑なので、後は別の方法でやる
        cm = GetComponent<CharaManager>();
        cm.CreateChara(new Vector2(-5.741f, -2.632f), CharaManager.CharaType.Player);

        for (int i = 0; i < mark.childCount; i++)
        {
            cm.CreateChara(mark.GetChild(i).position, (CharaManager.CharaType)types[i] + 1);
        }
    }

    //-------------------------------------------------
    // 更新処理(フレームごとに行う)
    //-------------------------------------------------
    private void Update()
    {
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
}