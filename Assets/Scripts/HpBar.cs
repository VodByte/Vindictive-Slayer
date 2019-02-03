using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    const int maxHp = 8;
    public float gap = 0.5f;
    public GameObject heart;
    private GameObject[] hearts = new GameObject[maxHp];
    private Vector2[] heartPosition = new Vector2[maxHp];
    private int nowHp = 0;
    private int prevHp = 0;

    private void Start()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            heartPosition[i] = transform.position + Vector3.right * gap * i;
            hearts[i] = Instantiate(heart, heartPosition[i], Quaternion.identity, gameObject.transform) as GameObject;
        }

        Invoke("GetInfo", 0.5f);
    }

    private void GetInfo()
    {
        prevHp = nowHp = GameInfo.PlayerInfo.iHp;
    }

    void Update()
    {
        nowHp = GameInfo.PlayerInfo.iHp;

        if (nowHp != prevHp)
        {
            foreach (var i in hearts)
            {
                i.SetActive(false);
            }

            for (int i = 0; i < nowHp; i++)
            {
                hearts[i].SetActive(true);
            }
        }
    }
}
