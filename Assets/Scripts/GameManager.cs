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
    [SerializeField] Transform mark;
    [SerializeField] int[] types;
    public GameObject pauseMenu;

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