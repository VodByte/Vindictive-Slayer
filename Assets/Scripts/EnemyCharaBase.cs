//---------------------------------------------------------------------
// EnemyCharaBase.cs
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 Enemy共通の情報と機能（abstract class）
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCharaBase : MonoBehaviour
{
    //------------------------------------------
    //変数宣言(Public)
    //------------------------------------------
    [HideInInspector]
    public Vector2 position;			// 敵の座標
    public GameObject hitEffect;		// 当たられたエフェクトのPrefab
    public GameObject deadEffect;       // 死亡エフェクトのPrefab
    public GameObject atkEffect;        // 攻撃エフェクトのPrefab
    public float moveSpeed;             // 右からくる速度(0.１に設定するのを推薦)
    public float RigorTime = 2.0f;		// 硬直時間（当たられたアニメーションがあったら、アニメーションの長さに応じて設定する）
    public float atkRange;              // 敵攻撃距離
    [HideInInspector]
    public bool isAtkReady = true;      // 敵の攻撃準備ができたのか
    public float atkRate = 0.1f;        // 敵の攻撃頻度
    public int atkPoint = 1;			// 敵の攻撃力
    public int iHp;                     // 敵HP（使えない敵もいる）
    public bool isOutScreen = false;    // ゲーム画面外
    // 敵の状態
    public enum EnemyStatus
    {
        normal,
        beAttacked,
        beKnocked,
        dead
    }
    protected EnemyStatus currentStatus = EnemyStatus.normal;       // 敵今の状態は
    public EnemyStatus CurrentStatus
    {
        get
        {
            return currentStatus;
        }
    }

    //------------------------------------------
    //変数宣言(Private/Protected)
    //------------------------------------------
    protected Animator ani;
    private float currentSpeed;				// 今使っている速度
    private float rigorTimer = 0.0f;		// タイマー、硬直状態の時間を
    private float deadSpeed = GameInfo.ScrollSpeed;     // 死亡したら、死体の移動速度
    protected float atkTimer = 0.0f;
    [SerializeField] private float knockSpeed = 0.0f;    // be knocked speed;
    private float knockUpTravel = 0.0f;
    [SerializeField] private float knockUpHeight = 1.0f;
    [SerializeField] private float knockUpSpeed = 2.0f;
    internal SpriteRenderer sr;

    //-------------------------------------------------
    // 初期化処理
    //-------------------------------------------------
    private void Awake()
    {
        position = transform.position;
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    //------------------------------------------
    // 更新処理(キャラクター共通)
    //------------------------------------------
    protected void UpdateEnemyChara()
    {
        // HPが0以下なら、死亡状態
        if (iHp <= 0)
        {
            currentStatus = EnemyStatus.dead;
        }

        // 攻撃頻度の更新
        if (atkTimer < atkRate)
        {
            atkTimer += Time.deltaTime;
        }
        else
        {
            isAtkReady = true;
        }

        // 状態に応じて処理を行う
        switch (CurrentStatus)
        {
            // 使われた速度は 敵自身の速度 + 画面移動の速度
            case EnemyStatus.normal:
                currentSpeed = moveSpeed + GameInfo.ScrollSpeed;
                break;
            // 止まり、硬直してから、普通状態に戻す
            case EnemyStatus.beAttacked:
                currentSpeed = 0.0f;
                rigorTimer += Time.deltaTime;
                if (rigorTimer >= RigorTime)
                {
                    rigorTimer = 0.0f;
                    currentStatus = EnemyStatus.normal;
                }
                break;
            // knockoutされた場合
            case EnemyStatus.beKnocked:
                currentSpeed = knockSpeed;
                // Y軸移動処理
                if (knockUpTravel <= Mathf.PI && knockUpTravel >= 0)
                {
                    knockUpTravel += Time.deltaTime * knockUpSpeed;
                }
                transform.position = new Vector2(transform.position.x, GameInfo.floorPos + Mathf.Sin(knockUpTravel) * knockUpHeight);

                if (transform.position.y <= GameInfo.floorPos)
                {
                    currentStatus = EnemyStatus.dead;
                }
                break;
            // 死亡、死亡アニメーションを再生
            case EnemyStatus.dead:
                sr.sortingOrder = -100;
                ani.SetBool("isDead", true);
                currentSpeed = deadSpeed;
                break;
        }

        // 敵の移動と実座標の更新
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
        position = transform.position;

        isOutScreen = CheckOutScreen();
    }

    //---------------------------------------------------------------
    // 敵がゲーム画面外か
    //---------------------------------------------------------------
    internal virtual bool CheckOutScreen()
    {
        return position.x < GameInfo.ScreenViewLeftEdgePos.x - 1.0f;
    }

    //---------------------------------------------------------------
    // 敵がknockoutされたら
    //---------------------------------------------------------------
    public virtual void SetKnockout()
    {
        currentStatus = EnemyStatus.beKnocked;
    }

    //---------------------------------------------------------------
    // 敵の死亡エフェクトを生成する（生成したから1.2秒の後でDelete）
    //---------------------------------------------------------------
    protected virtual void SetDeadAni()
    {
        //Destroy(Instantiate(deadEffect, transform.position, Quaternion.identity, gameObject.transform) as GameObject, 1.2f);
    }

    //------------------------------------------
    // 一回だけ行う、当たられたら時
    //------------------------------------------
    protected virtual void SetBeAtkAni()
    {
		// 当たったエフェクトを生成する（生成したから1.2秒の後でDelete）
		//Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity, gameObject.transform) as GameObject, 1.2f);
		// 敵が攻撃されたアニメーションを再生する
		ani.SetTrigger("beAttacked");
		// Knock back
		transform.position = new Vector2(GameInfo.PlayerInfo.transform.position.x + GameInfo.PlayerInfo.atkRange, transform.position.y);
    }

    //------------------------------------------
    // プレイヤーの入力をチェックする
    //------------------------------------------
    public abstract bool CheckPlayerInput(InputManager.AtkPattern atkPattern);

    //--------------------------------------------------------
    // プレイヤーの攻撃が失敗した場合、敵が特別な行動をする
    //--------------------------------------------------------
    public virtual bool CounterAttack() { return false; }

    //--------------------------------------------------------
    // 攻撃する
    //--------------------------------------------------------
    public virtual void Attack()
    {
        isAtkReady = false;
        atkTimer = 0.0f;
        ani.SetTrigger("attack");       // 攻撃あアニメーションを再生する
    }

    //--------------------------------------------------------
    // 敵の攻撃範囲を表す（GameViewに表示しない）
    //--------------------------------------------------------
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(sr.bounds.center, new Vector3(atkRange, 5.0f, 1.0f));
    }
}
