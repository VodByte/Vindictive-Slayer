using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 20.0f;
    [SerializeField]
    int atkPoint = 1;
    [SerializeField]
    float atkRange = 0.2f;
    [SerializeField]
    float reactionRange = 1.0f;
    Vector3 moveDir;
    bool isUsed = false;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;

        Vector3 diff = (Vector3)GameInfo.PlayerInfo.pos - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    private void Update()
    {
        if (!isUsed)
        {
            if (startPos.x <= GameInfo.PlayerInfo.pos.x)
            {
                moveDir = -transform.right * moveSpeed * Time.deltaTime;
            }
            else
            {
                moveDir = transform.right * moveSpeed * Time.deltaTime;
            }

            if (Vector3.Distance(transform.position, GameInfo.PlayerInfo.pos) < atkRange)
            {
                isUsed = true;
                GameInfo.PlayerInfo.BeAtked(atkPoint);
                GameInfo.PlayerInfo.PlayAudio(PlayerManager.AudioIndex.playerBeHitted);
            }

            if (Vector3.Distance(transform.position, GameInfo.PlayerInfo.pos) < reactionRange && InputManager.currentAtkPattern != InputManager.AtkPattern.NONE && InputManager.currentAtkPattern != InputManager.AtkPattern.LEFT)
            {
                isUsed = true;
                transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f));
                moveDir = -moveDir;
                moveSpeed *= 1.5f;
            }
        }

        transform.Translate(moveDir);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<SpriteRenderer>().bounds.center, new Vector3(reactionRange, 1.0f, 1.0f));
        Gizmos.DrawWireCube(GetComponent<SpriteRenderer>().bounds.center, new Vector3(atkRange, 1.0f, 1.0f));
    }
}