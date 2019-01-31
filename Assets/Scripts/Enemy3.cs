using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : EnemyCharaBase
{
    private void Update()
    {
        UpdateEnemyChara();
    }

    public override bool CheckPlayerInput(InputManager.AtkPattern atkPattern)
    {
        // 入力したキーは ↑ ↓ → のいずれなら
        if (atkPattern == InputManager.AtkPattern.LEFT)
        {
            // 一撃で死亡
            SetDeadAni();
            currentStatus = EnemyStatus.dead;
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
        moveSpeed = 0.0f;
       GameObject effect = Instantiate(atkEffect, transform.position + Vector3.up * sr.bounds.size.y * 0.4f, Quaternion.identity, gameObject.transform) as GameObject;
        Destroy(effect, effect.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
        Destroy(gameObject, GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }

    //---------------------------------------------------------------
    // 敵がゲーム画面外か
    //---------------------------------------------------------------
    internal override bool CheckOutScreen()
    {
        return position.x - sr.bounds.size.x > GameInfo.ScreenViewRightEdgePos.x;
    }
}