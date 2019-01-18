using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cudgel : MonoBehaviour
{
    public float gravity;
    public float beHitedSpeed;
    public float inputResponTime;

    private bool isActive = true;
    private float ySpeed = 0.0f;
    private float xSpeed = 0.0f;
    private float timer = 0.0f;

    void Start()
    {
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= inputResponTime)
        {
            isActive = false;
        }

        ySpeed = ySpeed + gravity * Time.deltaTime;
        transform.Translate(new Vector2(xSpeed, ySpeed));
    }

    void CheckPlayerInput(KeyCode x)
    {
        if (isActive)
        {
            if (x == KeyCode.RightArrow)
            {
                xSpeed = beHitedSpeed;
            }
        }
    }
}