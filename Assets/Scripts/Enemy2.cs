//---------------------------------------------------------------------
// Enemy2.cs
// 赤鬼の機能設定(未完成)
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

    private int inputIndex = 0;

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

        bool isFirstTwoAtkSuccess = false;

        // 先頭の二回攻撃は、↑じゃないと、反撃される
        // いずれ反撃も攻撃も、した後敵が立ち去る
        if (inputIndex < 3)
        {
            if (atkPattern == InputManager.AtkPattern.UP)
            {
                isFirstTwoAtkSuccess = true;

                if (inputIndex == 1)
                {
                    // 落ちる木棒を生成する
                    Instantiate(club, transform.position + new Vector3(0.0f, GetComponent<SpriteRenderer>().bounds.size.y / 2.0f, 0.0f), Quaternion.identity, Camera.main.transform);
                }
            }
            else
            {
                isFirstTwoAtkSuccess = false;
            }
        }
        else
        {
            SetDeadAni();
            currentStatus = EnemyStatus.dead;
        }

        if (isFirstTwoAtkSuccess)
        {
            ++inputIndex;
            currentStatus = EnemyStatus.beAttacked;
            SetBeAtkAni();
        }
        else
        {
            isCounterAttack = true;
        }

        return isCounterAttack;
    }

    protected override void SetBeAtkAni()
    {
        //Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject, 1.2f);
        transform.position = new Vector2(GameInfo.PlayerInfo.transform.position.x + GameInfo.PlayerInfo.atkRange, transform.position.y);

        if (inputIndex < 2)
        {
            ani.SetTrigger("beAttacked");
        }
        else
        {
            ani.SetBool("isNoWeapon", true);
        }
    }

    //------------------------------------------
    // 概要：敵の反撃
    // 引数：なし
    //------------------------------------------
    public override bool CounterAttack()
    {
        bool result = false;
        return result;
    }
}