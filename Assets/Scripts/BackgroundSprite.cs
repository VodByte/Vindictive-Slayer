using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSprite : MonoBehaviour
{
    public float moveSpeed;

    private void Start()
    {
        moveSpeed *= Random.Range(1.0f, 1.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 20.0f - moveSpeed);
    }

    void Update()
    {
        gameObject.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * GameInfo.ScrollSpeed);

        bool isSightless = transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x < GameInfo.ScreenViewLeftEdgePos.x;
        if (isSightless)
        {
            Destroy(gameObject);
        }
    }
}