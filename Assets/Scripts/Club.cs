using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour 
{
    [HideInInspector]public Vector2 pos = Vector2.zero;
    public float atkRange = 1.0f;
    public float knockDistance = 1.0f;
    public float knockForce = 1.0f;
    public float AccMultiply = 0.2f;

    private float moveSpeed = 0.0f;

    private void Update()
    {
        moveSpeed += knockForce / AccMultiply;
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.World);

        // 画面外なら消す
        bool isOutScreen = transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x > GameInfo.ScreenViewRightEdgePos.x;
        if (isOutScreen)
        {
            Destroy(gameObject, 1.0f);
        }

        pos = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<SpriteRenderer>().bounds.center, new Vector3(atkRange, 1.0f, 1.0f));
    }
}