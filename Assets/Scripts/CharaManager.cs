//---------------------------------------------------------------------
// CharaManager.cs
// キャラクター間のやりとり
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 辛いもの食べたい
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaManager : MonoBehaviour
{
    //------------------------------------------
    //変数宣言(Public)
    //------------------------------------------
    public GameObject[] charaPrefabs;       // 各Prefabを格納用配列

    // ゲームにいるキャラクターの列挙型
    public enum CharaType
    {
        Player,
        Enemy0,
        Enemy1,
        Enemy2,
        Club
    }

    //------------------------------------------
    //変数宣言(Private)
    //------------------------------------------
    private List<EnemyCharaBase> enemyList = new List<EnemyCharaBase> { };  // ゲーム内いる敵を全部格納用List
    private List<int> clubAtkList = new List<int> { };

    //-------------------------------------------------------------
    // 更新
    //-------------------------------------------------------------
    void Update()
    {
        // listの更新
        UpdateList();

        // 敵がプレイヤーを攻撃できるなら、攻撃を行う
        HandleEnemyAtkPlayer();

        // プレイヤーが敵を攻撃しようとする
        HandlePlayerAtkEnemy();

        // 木棒の攻撃を行う
        if (clubAtkList.Count != 0)
        {
            foreach (var i in clubAtkList)
            {
                enemyList[i].SetKnockout();
            }
        }
    }

    //---------------------------------------------------------------
    // 概要：listの更新
    //---------------------------------------------------------------
    private void UpdateList()
    {
        // 毎回Listをクリアして、判定する
        clubAtkList.Clear();
        for (int i = 0; i < enemyList.Count; i++)
        {
            // 木棒攻撃List
            foreach (var x in transform.GetComponentsInChildren<Club>())
            {
                float club2EnemyDistance = Mathf.Abs(x.pos.x - enemyList[i].position.x);
                bool isDistanceReady = club2EnemyDistance <= x.atkRange;
                bool isStatusReady = enemyList[i].CurrentStatus != EnemyCharaBase.EnemyStatus.dead
                    && enemyList[i].CurrentStatus != EnemyCharaBase.EnemyStatus.beKnocked;

                if (isDistanceReady && isStatusReady)
                {
                    clubAtkList.Add(i);
                }
            }

            // 死んだら、画面の左側から1.0の距離を離したら、Delete
            bool isOutScreen = enemyList[i].position.x < GameInfo.ScreenViewLeftEdgePos.x - 1.0f;
            bool isDead = enemyList[i].CurrentStatus == EnemyCharaBase.EnemyStatus.dead;
            if (isOutScreen || isDead)
            {
                if(isOutScreen)
                {
                    Destroy(enemyList[i].gameObject,2.0f);
                }
                enemyList.RemoveAt(i);
            }
        }

    }

    //---------------------------------------------------------------
    // 概要：敵がプレイヤーを攻撃できるなら、攻撃を行う
    //---------------------------------------------------------------
    private void HandleEnemyAtkPlayer()
    {
        foreach (var i in enemyList)
        {
            if (CheckCanAtkPlayer(i))
            {
                i.Attack();

                // Enemy1 本体はダメージを与えない、発射したブレットはする
                if (i.GetComponent<Enemy1>() == null)
                {
                    GameInfo.PlayerInfo.BeAtked(i.atkPoint);
                    GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.playerBeHitted);
                }
            }
        }
    }

    //---------------------------------------------------------------
    // 概要：プレイヤーが敵を攻撃しようとする
    //---------------------------------------------------------------
    private void HandlePlayerAtkEnemy()
    {
        // プレイヤーの攻撃準備完了、かつ攻撃ボタンを押した
        if (InputManager.currentAtkPattern != InputManager.AtkPattern.NONE)
        {
            bool isReady = GameInfo.PlayerInfo.Attack();
            if (!isReady) return;

            // チェックするコライーダの用意
            Collider2D checkCollider = new Collider2D();

            if (InputManager.currentAtkPattern == InputManager.AtkPattern.LEFT)
            {
                checkCollider = GameInfo.PlayerInfo.gameObject.transform.GetChild(1).gameObject.GetComponent<CircleCollider2D>();

            }
            else
            {
                checkCollider = GameInfo.PlayerInfo.gameObject.transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>();
            }

            Collider2D[] enemies = new Collider2D[8];   // 衝突している敵を格納配列
            ContactFilter2D filter2D = new ContactFilter2D
            {
                layerMask = LayerMask.NameToLayer("Enemy"),
                useTriggers = true
            };
            bool isAllNull = true;  // この配列は空っぽなのか

            checkCollider.OverlapCollider(filter2D, enemies);
            for (int i = 0; i < 8; i++)
            {
                if (enemies[i] != null)
                {
                    var enemyInfo = enemies[i].gameObject.GetComponent<EnemyCharaBase>();
                    if (enemyInfo.CurrentStatus != EnemyCharaBase.EnemyStatus.dead) // 死亡した敵に攻撃しない
                    {
                        isAllNull = false;
                        // 反撃
                        bool isCounterAttack = enemyInfo.CheckPlayerInput(InputManager.currentAtkPattern);
                        if (isCounterAttack)
                        {
                            enemyInfo.CounterAttack();
                            GameInfo.PlayerInfo.BeAtked(enemyInfo.atkPoint);
                        }
                        GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordHitEnemy01);
                    }
                }
            }

            GetComponent<InputManager>().ResetAtkInfo();

            // 素振りで攻撃できる敵がいないなら
            if (isAllNull)
            {
                GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordGesture);
            }
            else
            {
                GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordHitEnemy01);
            }
        }
    }

    //---------------------------------------------------------------
    // 概要：敵はプレイヤーを攻撃できるかを判定
    // 戻り値：プレイヤーを攻撃可能のか
    // 引数：攻撃側の敵のEnemeyCharaBase
    //---------------------------------------------------------------
    private bool CheckCanAtkPlayer(EnemyCharaBase enemy)
    {
        // プレイヤーは敵の攻撃距離内のか、画面内のか
        float distance = Mathf.Abs(GameInfo.PlayerInfo.pos.x - enemy.position.x);    // 敵とプレイヤーの距離
        bool isTouchable = distance <= enemy.atkRange && enemy.position.x < GameInfo.ScreenViewRightEdgePos.x;

        // 敵が普通の状態であるのか
        bool isNormalStatus = enemy.CurrentStatus == EnemyCharaBase.EnemyStatus.normal;

        // 敵の攻撃準備ができたのか
        bool isEnemyReady = enemy.isAtkReady;

        // プレイヤが死亡したのか
        bool isPlayerDead = GameInfo.PlayerInfo.currentPlayerStatus == PlayerManager.PlayerStatus.dead;

        // 判定
        if (isTouchable && isNormalStatus && isEnemyReady && !isPlayerDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //---------------------------------------------------------------
    // 概要：キャラクターを生成する
    // 戻り値：なし
    // 引数：生成の位置、生成するキャラクターの種類
    //---------------------------------------------------------------
    public void CreateChara(Vector2 pos, CharaType chara)
    {
        if (chara == CharaType.Player)
        {
            Instantiate(charaPrefabs[(int)chara], pos, Quaternion.identity);
        }
        else if (chara == CharaType.Club)
        {
            Instantiate(charaPrefabs[(int)chara], pos, Quaternion.identity, transform);
        }
        else
        {
            GameObject obj = Instantiate(charaPrefabs[(int)chara], pos, Quaternion.identity) as GameObject;
            EnemyCharaBase eCb = obj.GetComponent<EnemyCharaBase>();
            enemyList.Add(eCb);
        }
    }
}