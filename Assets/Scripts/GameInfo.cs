#define Debug_On
//---------------------------------------------------------------------
// GameInfo.cs
// ゲーム統合情報
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 作成
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    //------------------------------------------
    // 変数宣言
    //------------------------------------------
    public static float ScrollSpeed = 1.0f;          // スクロールの移動速度
    public static Vector2 ScreenViewLeftEdgePos;    // 画面一番左端の位置情報
    public static Vector2 ScreenViewRightEdgePos;   // 画面一番右端の位置情報
    public static PlayerManager PlayerInfo;         // プレイヤ情報

    public Text playerInfoText;     // 画面に表示する文字

#if Debug_On
    private float testInput = 1;
#endif

    //-------------------------------------------------
    // 初期化処理
    //-------------------------------------------------
    void Start()
    {
        ScreenViewLeftEdgePos = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, 0.0f));
        ScreenViewRightEdgePos = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 0.0f));

        PlayerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    //-------------------------------------------------
    // 更新処理(フレームごとに行う)
    //-------------------------------------------------
    private void Update()
    {
        // プレイヤのHPを表示する
        playerInfoText.text = "Player HP:" + PlayerInfo.iHp.ToString();

        // 1を押したら、スクロール速度 = 本来の速度 * 0.25
        // 2を押したら、スクロール速度 = 本来の速度 * 0.5
        // 3を押したら、スクロール速度 = 本来の速度
#if Debug_On
        ScrollSpeed = 1.0f * testInput;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            testInput = 1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            testInput = 0.05f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            testInput = 0.025f;
        }
#endif
    }
}