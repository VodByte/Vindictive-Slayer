//---------------------------------------------------------------------
// Enemy0.cs
// 餓鬼（EnemyCharaBaseをインヘリタンス）
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 描くべきもの山ほど溜まってる
//---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0 : EnemyCharaBase
{
    //--------------
    // 更新
    //--------------
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

		// 入力したキーは ↑ ↓ → のいずれなら
        if (atkPattern == InputManager.AtkPattern.DOWN || (atkPattern == InputManager.AtkPattern.LEFT && transform.position.x < GameInfo.PlayerInfo.pos.x))
        {
			// 一撃で死亡
            SetDeadAni();
            currentStatus = EnemyStatus.dead;
            GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordHitEnemy01);
        }
        else
        {
            GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordGesture);
            ani.SetTrigger("Def");
        }

        return isCounterAttack;
    }
}
