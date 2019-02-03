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

public class GameInfo : MonoBehaviour
{
    //------------------------------------------
    // 変数宣言
    //------------------------------------------
	public float initalScrollSpeed = 1.0f;
    [HideInInspector]public static float ScrollSpeed = 1.0f;          // スクロールの移動速度
    public static Vector2 ScreenViewLeftEdgePos;    // 画面一番左端の位置情報
    public static Vector2 ScreenViewRightEdgePos;   // 画面一番右端の位置情報
    public static PlayerManager PlayerInfo;         // プレイヤ情報
    public const  float floorPos = -3.77f;
    [HideInInspector] public static int total_Score = 0;
    public Transform scoreTextTransform;
    [HideInInspector] public static Vector2 scoreTextPos;
    [HideInInspector] public static bool isStageClear;
    //-------------------------------------------------
    // 初期化処理
    //-------------------------------------------------
    void Start()
    {
		ScrollSpeed = initalScrollSpeed;
        ScreenViewLeftEdgePos = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, 0.0f));
        ScreenViewRightEdgePos = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 0.0f));
        PlayerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        scoreTextPos = scoreTextTransform.position;
    }

    void Update()
    {
        GameInfo_StaticClass.total_Score = total_Score;
        GameInfo_StaticClass.stageTime = Time.timeSinceLevelLoad;
    }
}