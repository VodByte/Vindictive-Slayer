using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public ContactFilter2D contactFilter2D;

    // Private
    new Collider2D collider = new Collider2D();
    AudioSource beHitSE;
    [SerializeField]
    float moveSpeed = 20.0f;
    [SerializeField]
    int atkPoint = 1;
    [SerializeField]
    float reactionRange = 1.0f;
    bool isInvail = false;
    Vector2 moveDir;

    private void Start()
    {
        beHitSE = GetComponent<AudioSource>();
        collider = GetComponent<CircleCollider2D>();
        moveDir = -transform.up * moveSpeed * Time.deltaTime;
         var dir = ((Vector3)GameInfo.PlayerInfo.pos + Vector3.up * 0.5f - transform.position).normalized;
		var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.localRotation = Quaternion.AngleAxis(angle + 90.0f, Vector3.forward);
    }

    private void Update()
    {
        if (!isInvail)
        {
            if (CheckTouchPlayer())
            {
                GameInfo.PlayerInfo.BeAtked(atkPoint);
                GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.playerBeHitted);
                Destroy(gameObject);
                return;
            }

            // If be hitted
            bool isOverlayX = Mathf.Abs(transform.position.x - GameInfo.PlayerInfo.pos.x) < reactionRange;
            bool isOverlayY = Mathf.Abs(transform.position.y - GameInfo.PlayerInfo.pos.y) < reactionRange;
            if (isOverlayX && isOverlayY && InputManager.currentAtkPattern != InputManager.AtkPattern.NONE && InputManager.currentAtkPattern != InputManager.AtkPattern.LEFT)
            {

                SpriteRenderer sr =  GetComponent<SpriteRenderer>();
                Color color = sr.color;
                sr.color = new Color(color.r / 2.0f, color.g / 2.0f, color.b / 2.0f);
                isInvail = true;
                Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject, 2.0f);
                moveDir = transform.right * moveSpeed * Time.deltaTime;
                beHitSE.Play();
                transform.localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0.0f, 360.0f) + 90.0f, Vector3.forward);
                moveSpeed *= 1.2f;
            }
        }

        // Out Screen
        if (transform.position.x < GameInfo.ScreenViewLeftEdgePos.x - 2.0f || transform.position.x > GameInfo.ScreenViewRightEdgePos.x + 2.0f)
        {
            Destroy(gameObject);
            return;
        }
       
        transform.Translate(moveDir);
    }

    private bool CheckTouchPlayer()
    {
        Collider2D[] touchCollider = new Collider2D[2];
        collider.OverlapCollider(contactFilter2D, touchCollider);
        foreach (var i in touchCollider)
        {
            if (i != null && i.gameObject.name == "Player(Clone)" && GameInfo.PlayerInfo.currentPlayerStatus != PlayerManager.PlayerStatus.invincible)
            {
                return true;
            }
        }

        return false;
    }
}