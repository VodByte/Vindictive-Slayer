using UnityEngine;

public class Club : MonoBehaviour 
{
    [HideInInspector]public Vector2 pos = Vector2.zero;
    public float gravity = 9.8f;
    public float atkRange = 1.0f;
    public float knockForce = 1.0f;
    public float AccMultiply = 0.2f;

    private float moveSpeed = 0.0f;
    private bool isHitRight = false;
    private float dropSpeed = 0.0f;
    private bool isAttackable = false;

    private void Update()
    {
        CheckSightless();

        if (InputManager.currentAtkPattern == InputManager.AtkPattern.RIGHT)
        {
            isHitRight = true;
            isAttackable = true;
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

    }

    private void CheckSightless()
    {
        // 画面外なら消す
        bool isBelowFloor = transform.position.y < GameInfo.floorPos;
        bool isOutScreen = transform.position.x > GameInfo.ScreenViewRightEdgePos.x;
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

    public bool CanDamage(EnemyCharaBase enemy)
    {
        bool isInRagne = Mathf.Pow((enemy.position.x - transform.position.x), 2) 
        + Mathf.Pow((enemy.position.y - transform.position.y), 2) <= atkRange * atkRange;

        bool isStatusReady = enemy.CurrentStatus != EnemyCharaBase.EnemyStatus.dead
                    && enemy.CurrentStatus != EnemyCharaBase.EnemyStatus.beKnocked;

        return isInRagne && isStatusReady && isAttackable;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(GetComponent<SpriteRenderer>().bounds.center, new Vector3(atkRange, 1.0f, 1.0f));
    }
}