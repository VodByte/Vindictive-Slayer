//---------------------------------------------------------------------
// InputManager.cs
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 入力は有効かのチェックと格納
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum AtkPattern
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    };
    public static AtkPattern currentAtkPattern = AtkPattern.NONE;

    public enum MovePattern
    {
        LEFT,
        RIGHT,
        NONE
    }
    public static MovePattern currentMovePattern = MovePattern.NONE;

    // 有効入力の配列
    private KeyCode[] atkKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    private KeyCode[] moveKeys = new KeyCode[] {KeyCode.A, KeyCode.D};

	// 今キーを押しているのか
    private bool isHoldingDown = false;
    private bool isAtked = false;

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.anyKey)
        {
            isHoldingDown = true;

            // キーを押したら、そのキーは有効入力のかを検索する
            // 移動キー
            for (int i = 0; i < moveKeys.Length; i++)
            {
                if (Input.GetKeyDown(moveKeys[i]))
                {
                    switch (moveKeys[i])
                    {
                        case KeyCode.A:
                            currentMovePattern = MovePattern.LEFT;
                            break;
                        case KeyCode.D:
                            currentMovePattern = MovePattern.RIGHT;
                            break;
                    }
                }
            }
            // 攻撃キー
            for (int i = 0; i < atkKeys.Length; i++)
            {
                if (Input.GetKeyDown(atkKeys[i]) && !isAtked)
                {
                    switch (atkKeys[i])
                    {
                        case KeyCode.UpArrow:
                            currentAtkPattern = AtkPattern.UP;
                            break;
                        case KeyCode.DownArrow:
                            currentAtkPattern = AtkPattern.DOWN;
                            break;
                        case KeyCode.LeftArrow:
                            currentAtkPattern = AtkPattern.LEFT;
                            break;
                        case KeyCode.RightArrow:
                            currentAtkPattern = AtkPattern.RIGHT;
                            break;
                    }
                    isAtked = true;
                }
            }
        }

		// キーを離したら、格納したものをクリアする
        if (!Input.anyKey && isHoldingDown)
        {
            isHoldingDown = false;
            currentMovePattern = MovePattern.NONE;
        }
    }

    public void ResetAtkInfo()
    {
        currentAtkPattern = InputManager.AtkPattern.NONE;
        isAtked = false;
    }
}