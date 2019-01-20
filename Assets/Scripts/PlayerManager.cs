//---------------------------------------------------------------------
// PlayerManager.cs
// 我々のヒーローの機能
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 Fallout76何回やってもクソゲ！
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //------------------------------------------
    //変数宣言(public)
    //------------------------------------------
    public int iHp;                         // プレイヤーのHP
    [Range(0.0f, 10.0f)]
    public float moveSpeed = 0.2f;
    public float atkRange;                  // プレイヤーの攻撃範囲
    [Range(0.1f,2.0f)]
    public float atkRate = 2.0f;                   // プレイヤーの攻撃頻度
    [HideInInspector]public bool isAtkReady = true;
    public float beAtkedInvincibleTime;     // 攻撃を受けたら、無敵時間
    public float blinkCycleTime = 0.05f;     // 無敵中、Sprite点滅の間隔
    [HideInInspector]
    public Vector2 pos;         // プレイヤーの位置を格納
    public enum PlayerStatus    // プレイヤーの状態
    {
        normal,
        invincible,
        dead
    }
    public PlayerStatus currentPlayerStatus = PlayerStatus.normal;

    public enum AudioIndex      // Player用SFX配列の要素番号を表示する用
    {
        swordGesture,       // プレイヤーの武器が何も当たってない
        swordHitEnemy01,    // 武器が敵に当たった
        playerBeHitted,     // プレイヤーが攻撃された
    }

    //------------------------------------------
    // 変数宣言(private)
    //------------------------------------------
    private Animator ani;      // Player GameObjectにアタッチしたAnimator
    private float initialSpeed = GameInfo.ScrollSpeed;      // 背景の初期移動速度
    private float rigorTime;        // 当たられたら硬直時間(今はadventurer_BeAtkedアニメーションの長さで設定する)
    private float invincibleTimer = 0.0f;       // 無敵時間を計るタイマー
    private float blinkTimer = 0.0f;    // Sprite点滅を計るタイマー
    private float atkTimer = 0.0f;
    private AudioSource[] audios;       // Player用SFX配列
    private int prevUpIndex = 0;
    private int prevMidIndex = 0;
    private int prevDownIndex = 0;
    //-------------------------------------------------
    // 初期化処理
    //-------------------------------------------------
    private void Start()
    {
        ani = GetComponent<Animator>();

        for (int i = 0; i < ani.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (ani.runtimeAnimatorController.animationClips[i].name == "Player_Hurt")
            {
                rigorTime = ani.runtimeAnimatorController.animationClips[i].length;     // Animator内の"Player_Hurt"アニメーション長さを取得する
            }
        }
        audios = GetComponents<AudioSource>();
    }

    //-------------------------------------------------
    // 更新処理(フレームごとに行う)
    //-------------------------------------------------
    private void Update()
    {
        pos = transform.position;       // 座標更新

        // 攻撃頻度の更新
        if (atkTimer < atkRate)
        {
            atkTimer += Time.deltaTime;
        }
        else
        {
            isAtkReady = true;
        }

        // 移動処理
        int dir = 0;
        switch (InputManager.currentMovePattern)
        {
            case InputManager.MovePattern.LEFT:
                dir = -1;
                break;
            case InputManager.MovePattern.RIGHT:
                dir = 1;
                break;
            case InputManager.MovePattern.NONE:
                dir = 0;
                break;
        }
        transform.Translate(Vector2.right * moveSpeed * dir * Time.deltaTime);
        if (transform.position.x <= GameInfo.ScreenViewLeftEdgePos.x)
        {
            transform.position = new Vector2(GameInfo.ScreenViewLeftEdgePos.x, transform.position.y);
        }
        else if (transform.position.x >= GameInfo.ScreenViewRightEdgePos.x)
        {
            transform.position = new Vector2(GameInfo.ScreenViewRightEdgePos.x, transform.position.y);
        }

        // 状態に応じて処理を行う
        switch (currentPlayerStatus)
        {
            case PlayerStatus.normal:
                break;
            case PlayerStatus.invincible:
                UpdateInvincibleStatus();
                break;
            case PlayerStatus.dead:
                break;
        }
    }

    //------------------------------------------
    // 概要：無敵状態中の情報処理(フレームごとに行う)
    // 戻り値：なし
    // 引数：なし
    //------------------------------------------
    private void UpdateInvincibleStatus()
    {
        invincibleTimer += Time.deltaTime;

        // 硬直アニメーションを過ごしたなら
        if (invincibleTimer < rigorTime)
        {
            GameInfo.ScrollSpeed -= initialSpeed / rigorTime * Time.deltaTime;        // 段々背景を止める
        }
        else
        {
            GameInfo.ScrollSpeed = initialSpeed;        // 背景を動かす

            // 無敵時間を過ごしたなら
            if (invincibleTimer >= beAtkedInvincibleTime)
            {
                currentPlayerStatus = PlayerStatus.normal;
                GetComponent<SpriteRenderer>().enabled = true;
                invincibleTimer = 0.0f;
            }
            else
            {
                // プレイヤーのSpriteを点滅させる
                blinkTimer += Time.deltaTime;
                if (blinkTimer < blinkCycleTime)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    blinkTimer = 0.0f;
                }
            }
        }
    }

    //------------------------------------------
    // 概要：撃たれた反応
    // 戻り値：なし
    // 引数：攻撃側の攻撃力
    //------------------------------------------
    public void BeAtked(int damage)
    {
        // 無敵なら、以下の処理はしない
        if (currentPlayerStatus == PlayerStatus.invincible) return;

        iHp -= damage;
        if (iHp <= 0)
        {
            iHp = 0;
            audios[(int)AudioIndex.playerBeHitted].Play();
            currentPlayerStatus = PlayerStatus.dead;
            ani.SetBool("isDead", true);
            GameInfo.ScrollSpeed = 0.0f;
        }
        else
        {
            ani.SetTrigger("beAttatked");
            currentPlayerStatus = PlayerStatus.invincible;
        }
    }

    //------------------------------------------
    // 概要：攻撃する
    // 戻り値：プレイヤーは攻撃できるのか
    // 引数：なし
    //------------------------------------------
    public bool Attack()
    {
        if (currentPlayerStatus != PlayerManager.PlayerStatus.dead && isAtkReady)
        {
            isAtkReady = false;
            atkTimer = 0.0f;

            switch (InputManager.currentAtkPattern)
            {
                case InputManager.AtkPattern.UP:
                    {
                        int index = 0;
                        do
                        {
                            index = UnityEngine.Random.Range(1, 4);
                        } while (index == prevUpIndex);
                        prevUpIndex = index;

                        switch (index)
                        {
                            case 1:
                                ani.SetTrigger("UpAtk01");
                                break;
                            case 2:
                                ani.SetTrigger("UpAtk02");
                                break;
                            case 3:
                                ani.SetTrigger("UpAtk03");
                                break;
                        }
                    }
                    break;
                case InputManager.AtkPattern.RIGHT:
                    {
                        int index = 0;
                        do
                        {
                            index = UnityEngine.Random.Range(1, 4);
                        } while (index == prevMidIndex);
                        prevMidIndex = index;

                        switch (index)
                        {
                            case 1:
                                ani.SetTrigger("MidAtk01");
                                break;
                            case 2:
                                ani.SetTrigger("MidAtk02");
                                break;
                            case 3:
                                ani.SetTrigger("MidAtk03");
                                break;
                        }
                    }
                    break;
                case InputManager.AtkPattern.LEFT:
                    break;
                case InputManager.AtkPattern.DOWN:
                    {
                        int index = 0;
                        do
                        {
                            index = UnityEngine.Random.Range(1, 4);
                        } while (index == prevDownIndex);
                        prevDownIndex = index;

                        switch (index)
                        {
                            case 1:
                                ani.SetTrigger("DownAtk01");
                                break;
                            case 2:
                                ani.SetTrigger("DownAtk02");
                                break;
                            case 3:
                                ani.SetTrigger("DownAtk03");
                                break;
                        }
                    }
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //------------------------------------------
    // 概要：SFXの再生
    // 戻り値：なし
    // 引数：再生するSFXのindex
    //------------------------------------------
    public void PlayAudio(AudioIndex audio)
    {
        if (currentPlayerStatus != PlayerManager.PlayerStatus.dead)
        {
            audios[(int)audio].Play();
        }
    }

    //--------------------------------------------------------
    // 敵の攻撃範囲を表す（GameViewに表示しない）
    //--------------------------------------------------------
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}