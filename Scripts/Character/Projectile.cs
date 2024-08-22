using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 targetPosition;
    public float speed;
    public float AddAngle;

    public void InitProjectileRotation(Vector2 target)
    {
        targetPosition = target;

        // 투사체의 방향을 설정
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + AddAngle));
    }

    void Update()
    {
        if (targetPosition != null)
        {
            // 투사체를 목표지점으로 이동
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 목표지점에 도달했는지 확인
            if ((Vector2)transform.position == targetPosition)
            {
                OnHit();
            }
        }
    }

    private void OnHit()
    {
        Destroy(gameObject);
    }
}
