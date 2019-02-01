using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMark : MonoBehaviour
{
    private Vector2 moveTarget;
    private float moveSpeed = 0.0f;
    public float moveForce = 0.1f;

    private void Start()
    {
        moveTarget = GameInfo.scoreTextPos;
    }

    private void Update()
    {
        moveSpeed += moveForce * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveTarget) < 0.2f)
        {
            Destroy(gameObject);
        }
    }
}
