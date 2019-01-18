//---------------------------------------------------------------------
// Enemy1.cs
// ???（EnemyCharaBaseをインヘリタンス）
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 ビールもっと安くなったらいいなぁ
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyCharaBase
{
    //------------------------------------------
    // 更新処理
    //------------------------------------------
    void Update()
    {
        UpdateEnemyChara();
    }

    //------------------------------------------
    // 概要：プレイヤーの入力をチェックする
    // 戻り値：反撃するのか
    // 引数：プレイヤーが入力したキー
    //------------------------------------------
    public override bool CheckPlayerInput(InputManager.AtkPattern atkPattern)
    {
        bool isCounterAttack = false;

        if (atkPattern != InputManager.AtkPattern.LEFT && atkPattern != InputManager.AtkPattern.NONE)
        {
            --iHp;
            if (iHp > 0)
            {
                currentStatus = EnemyStatus.beAttacked;
                SetBeAtkAni();
            }
            else
            {
                currentStatus = EnemyStatus.dead;
                SetDeadAni();
            }
        }

        return isCounterAttack;
    }
}