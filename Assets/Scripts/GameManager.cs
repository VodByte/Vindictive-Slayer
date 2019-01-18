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

public class GameManager : MonoBehaviour
{
    //------------------------------------------
    //変数宣言
    //------------------------------------------
    CharaManager cm;

    //-------------------------------------------------
    // 初期化処理
    //-------------------------------------------------
    private void Awake()
    {
        // 敵を配置する。このやり方は雑なので、後は別の方法でやる
        cm = GetComponent<CharaManager>();

        cm.CreateChara(new Vector2(-5.741f, -2.632f), CharaManager.CharaType.Player);
        cm.CreateChara(new Vector2(5.1f, -3.86f), CharaManager.CharaType.Enemy1);
        cm.CreateChara(new Vector2(-11f, -2.87f), CharaManager.CharaType.Club);

        //cm.CreateChara(new Vector2(10.1f, -2.46f), CharaManager.CharaType.Enemy0);
        //cm.CreateChara(new Vector2(10.1f + 2.5f, -2.46f), CharaManager.CharaType.Enemy0);
        //cm.CreateChara(new Vector2(10.1f + 5f, -2.46f), CharaManager.CharaType.Enemy0);
        //cm.CreateChara(new Vector2(35.15f, -2.46f), CharaManager.CharaType.Enemy1);
        //cm.CreateChara(new Vector2(35.15f + 1.5f, -2.46f), CharaManager.CharaType.Enemy0);

        //cm.CreateChara(new Vector2(40.0f + 1.5f, -2.46f), CharaManager.CharaType.Enemy1);
        //cm.CreateChara(new Vector2(40.0f + 3f, -2.46f), CharaManager.CharaType.Enemy1);
        //cm.CreateChara(new Vector2(40.0f + 4.5f, -2.46f), CharaManager.CharaType.Enemy1);
    }

    //-------------------------------------------------
    // 更新処理(フレームごとに行う)
    //-------------------------------------------------
    private void Update()
    {

#if Debug_On
        // R を押したら、Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
#endif

    }
}