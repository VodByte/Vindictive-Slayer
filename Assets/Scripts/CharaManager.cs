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
        Club,
        Enemy0,
        Enemy1,
        Enemy2
    }

    //------------------------------------------
    //変数宣言(Private)
    //------------------------------------------
    private List<EnemyCharaBase> enemyList = new List<EnemyCharaBase> { };  // ゲーム内いる敵を全部格納用List
    private List<int> playerCanAtkEnemyList = new List<int> { };            // そのListの中に、プレイヤーが攻撃可能の敵の要素番号を格納用List
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
        playerCanAtkEnemyList.Clear();
        clubAtkList.Clear();
        for (int i = 0; i < enemyList.Count; i++)
        {
            // 画面の左側から5.0の距離を離したら、Delete
            bool isOutScreen = enemyList[i].position.x < GameInfo.ScreenViewLeftEdgePos.x - 5.0f;
            if (isOutScreen)
            {
                Destroy(enemyList[i].gameObject);
                enemyList.RemoveAt(i);
            }

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

            // 敵とプレイヤーの距離
            float enemy2playerDistance = Mathf.Abs(GameInfo.PlayerInfo.pos.x - enemyList[i].position.x);
            // その距離はプレイヤーの攻撃範囲内のか
            bool isPlayerCanAtkEnemy = enemy2playerDistance <= GameInfo.PlayerInfo.atkRange;
            // プレイヤーが攻撃できる敵の情報を格納用Listに追加
            if (isPlayerCanAtkEnemy)
            {
                playerCanAtkEnemyList.Add(i);
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
                GameInfo.PlayerInfo.BeAtked(i.atkPoint);
                GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.playerBeHitted);
            }
        }
    }



    //---------------------------------------------------------------
    // 概要：プレイヤーが敵を攻撃しようとする
    //---------------------------------------------------------------
    private void HandlePlayerAtkEnemy()
    {
        if (InputManager.currentAtkPattern != InputManager.AtkPattern.NONE)
        {
            bool isPlayerCanAtk = GameInfo.PlayerInfo.Attack();

            if (isPlayerCanAtk)
            {
                // 攻撃できる敵がいるなら
                if (playerCanAtkEnemyList.Count != 0)
                {
                    foreach (var i in playerCanAtkEnemyList)
                    {
                        // 敵が反撃するのか(Enemy0,Enemy1は反撃機能がない)
                        bool isEnemyCounterAttack = enemyList[i].CheckPlayerInput(InputManager.currentAtkPattern);
                        if (isEnemyCounterAttack)
                        {
                            enemyList[i].CounterAttack();       // 反撃をする
                            GameInfo.PlayerInfo.BeAtked(enemyList[i].atkPoint);     // プレイヤーのダメージ処理
                        }
                        // プレイヤーの武器が敵に当たったSFX
                        GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordHitEnemy01);
                    }
                }
                // 素振りで攻撃できる敵がいないなら
                else
                {
                    GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.swordGesture);
                }
            }
            GetComponent<InputManager>().ResetAtkInfo();
        }
    }

    //---------------------------------------------------------------
    // 概要：敵はプレイヤーを攻撃できるかを判定
    // 戻り値：プレイヤーを攻撃可能のか
    // 引数：攻撃側の敵のEnemeyCharaBase
    //---------------------------------------------------------------
    private bool CheckCanAtkPlayer(EnemyCharaBase enemy)
    {
        // プレイヤーは敵の攻撃距離内のか
        float distance = Mathf.Abs(GameInfo.PlayerInfo.pos.x - enemy.position.x);    // 敵とプレイヤーの距離
        bool isTouchable = distance <= enemy.atkRange;

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
