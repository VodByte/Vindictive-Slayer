using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour 
{
    [HideInInspector]public Vector2 pos = Vector2.zero;
    public float gravity = 9.8f;
    public float atkRange = 1.0f;
    public float knockForce = 1.0f;
    public float AccMultiply = 0.2f;

    private float moveSpeed = 0.0f;
    private bool isBelowFloor = false;
    private bool isHitRight = false;
    private float dropSpeed = 0.0f;

    private void Update()
    {
        isBelowFloor = transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y * 0.5f < GameInfo.floorPos;
        if (!isBelowFloor && InputManager.currentAtkPattern == InputManager.AtkPattern.RIGHT)
        {
            isHitRight = true;
        }

        if (isHitRight)
        {
            HitRight();
        }
        else
        {
            dropSpeed += gravity * Time.deltaTime;
            transform.position -= new Vector3(0.0f, dropSpeed, 0.0f);
        }

        CheckSightless();
    }

    private void CheckSightless()
    {
        // 画面外なら消す
        bool isOutScreen = transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x > GameInfo.ScreenViewRightEdgePos.x;
        if (isOutScreen || isBelowFloor)
        {
            Destroy(gameObject);
        }
    }

    private void HitRight()
    {
        moveSpeed += knockForce / AccMultiply;
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.World);
        pos = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<SpriteRenderer>().bounds.center, new Vector3(atkRange, 1.0f, 1.0f));
    }
}