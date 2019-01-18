//---------------------------------------------------------------------
// ScrollMap.cs
// スクロール機能
// 作成者　18CU0116 左国力
// 作成日　2018-12-10
// 更新履歴
// 2018年12月10日 作成
//---------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMap : MonoBehaviour
{
    //------------------------------------------
    // 変数宣言
    //------------------------------------------
    // BG用Spriteの集合
    public GameObject[] foregroundLayers;
    public GameObject[] middlegroundLayers;
    public GameObject[] backgroundLayers;

    // 設定された時間ごとに、生成するかを判定する
    public float foregroundLayerGap = 0.5f;
    public float middleLayerGap = 0.5f;
    public float backgroundLayersGap = 0.5f;

    // 生成する可能性
    [Range(0.0f, 100.0f)] public float foregroundProp = 50.0f;
    [Range(0.0f, 100.0f)] public float middlegroundProp = 50.0f;
    [Range(0.0f, 100.0f)] public float backgroundProp = 50.0f;


    public Vector2 scaleRandom;
    public Vector2 createPos;
    public Transform parent;

    private float foregroundTimer = 0.0f;
    private float middlegroundTimer = 0.0f;
    private float backgroundTimer = 0.0f;

    private int foregroundPrevIndex = 0;
    private int middlegroundPrevIndex = 0;
    private int backgroundPrevIndex = 0;

    //------------------------------------------
    // 更新
    //------------------------------------------
    private void Update()
    {
        CreateBGSprite(foregroundLayers, foregroundLayerGap,ref foregroundProp, ref foregroundTimer, ref foregroundPrevIndex);
        CreateBGSprite(middlegroundLayers, middleLayerGap, ref middlegroundProp, ref middlegroundTimer, ref middlegroundPrevIndex);
        CreateBGSprite(backgroundLayers, backgroundLayersGap, ref backgroundProp, ref backgroundTimer, ref backgroundPrevIndex);
    }

    private void CreateBGSprite(GameObject[] layers, float timeGap, ref float prop, ref float timer , ref int prevIndex)
    {
        timer += Time.deltaTime;
        bool flag = Random.Range(0.0f, 100.0f) < prop;

        if (timer > timeGap)
        {
            timer = 0.0f;

            if (flag)
            {
                int index = 0;
                // Randomで要素番号を取得する
                do
                {
                    index = Random.Range(0, layers.Length);
                } while (index == prevIndex);
                prevIndex = index;

                GameObject bg = Instantiate(layers[index], createPos, Quaternion.identity, parent) as GameObject;

                float newScaleX = bg.transform.localScale.x * Random.Range(1.0f, scaleRandom.x);
                float newScaleY = bg.transform.localScale.y * Random.Range(1.0f, scaleRandom.y);
                bg.transform.localScale = new Vector2(newScaleX, newScaleY);
            }
        }
    }
}