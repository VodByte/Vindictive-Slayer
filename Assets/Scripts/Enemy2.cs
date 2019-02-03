//---------------------------------------------------------------------
// Enemy2.cs
// 赤鬼の機能設定
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 しんどい
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyCharaBase
{
    //------------------------------------------
    //変数宣言
    //------------------------------------------
    public GameObject club;     // 落ちる木棒

    private int inputSuccessCount = 0;
    private bool isDropClub = false;

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
        // isClubDrop
        if (transform.position.x > GameInfo.PlayerInfo.pos.x && atkPattern == InputManager.AtkPattern.UP && inputSuccessCount < 3)
        {
            ++inputSuccessCount;
            if (inputSuccessCount == 2)
            {
                isDropClub = true;
                ani.SetBool("isNoWeapon", true);
                Instantiate(club, transform.position + Vector3.up * 2.0f, Quaternion.identity, Camera.main.transform);
            }
        }

        if (!isDropClub)
        {
            if (atkPattern == InputManager.AtkPattern.UP)
            {
                currentStatus = EnemyStatus.beAttacked;
                SetBeAtkAni();
                GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordHitEnemy01);
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (atkPattern != InputManager.AtkPattern.LEFT)
            {
                --iHp;
                if (iHp <= 0)
                {
                    SetDeadAni();
                    currentStatus = EnemyStatus.dead;
                }
                else
                {
                    currentStatus = EnemyStatus.beAttacked;
                }
                GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordHitEnemy01);
            }
        }

        return false;
    }

    protected override void SetBeAtkAni()
    {
        ani.SetTrigger("beAttacked");
    }

    //------------------------------------------
    // 概要：敵の反撃
    // 引数：なし
    //------------------------------------------
    public override bool CounterAttack()
    {
        Attack();
        bool result = false;
        return result;
    }
}