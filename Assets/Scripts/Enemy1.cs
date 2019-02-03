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
    public GameObject bullet;
    public float deadUpForce = 1.0f;

    private float deadUpSpeed = 0.0f;

    //------------------------------------------
    // 更新処理
    //------------------------------------------
    void Update()
    {
        UpdateEnemyChara();

        if (currentStatus == EnemyStatus.dead)
        {
            deadUpSpeed += deadUpForce * Time.deltaTime;
            transform.position += new Vector3(0.0f, deadUpSpeed * Time.deltaTime, 0.0f);
        }
    }

    //------------------------------------------
    // 概要：プレイヤーの入力をチェックする
    // 戻り値：反撃するのか
    // 引数：プレイヤーが入力したキー
    //------------------------------------------
    public override bool CheckPlayerInput(InputManager.AtkPattern atkPattern)
    {

        if (atkPattern == InputManager.AtkPattern.UP)
        {
            --iHp;
            currentStatus = EnemyStatus.dead;
            SetDeadAni();
            GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordHitEnemy01);
        }

        return false;
    }

    //--------------------------------------------------------
    // 攻撃する
    //--------------------------------------------------------
    public override void Attack()
    {
        isAtkReady = false;
        atkTimer = 0.0f;
        ani.SetTrigger("attack");       // 攻撃あアニメーションを再生する
        Instantiate(bullet, transform.GetChild(0).transform.position, Quaternion.identity);
    }

    //---------------------------------------------------
    // 敵の死亡エフェクトを生成する
    //---------------------------------------------------
    protected override void SetDeadAni()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public override void SetKnockout()
    {
        --iHp;
        currentStatus = EnemyStatus.dead;
        SetDeadAni();
    }

}