using UnityEngine;

public class PenetratingArrow : PlayerSkill
{
    private GameObject arrowEffectPrefab;

    public PenetratingArrow() : base(
        "관통하는 화살",
        "Archer는 강력한 화살을 쏘아 기본피해()+공격피해*스킬계수의 데미지를 직선경로의 모든 적에게 물리적 피해를 입힌다.",
        5,
        new int[] { 12, 24, 36, 48, 60 },
        new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f })
    {
    }

    public void SetEffectPrefab(GameObject prefab)
    {
        arrowEffectPrefab = prefab;
    }

    public override void UseSkill(Hero caster)
    {
        Archer archer = caster as Archer;
        if (archer == null)
        {
            Debug.LogError("Caster is not an Archer!");
            return;
        }

        int baseDamage = BaseDamage[Level - 1];
        float coefficient = Coefficients[Level - 1];
        float totalDamage = baseDamage + (archer.attackDamage * coefficient);

        if (archer._target != null)
        {
            Vector2 direction = (archer._target.transform.position - archer.transform.position).normalized;
            float maxDistance = 100f; // 스킬의 최대 사거리

            // 이펙트는 첫 번째 대상에게만 생성
            CreateArrowEffect(archer._target.transform.position, direction);

            Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in allEnemies)
            {
                Vector2 toEnemy = enemy.transform.position - archer.transform.position;
                float dotProduct = Vector2.Dot(toEnemy.normalized, direction);

                if (dotProduct > 0.99f && toEnemy.magnitude <= maxDistance)
                {
                    enemy.TakeDamage(archer, totalDamage);
                }
            }
        }
        else
        {
            Debug.LogWarning("No target found for Penetrating Arrow skill.");
        }

        archer.Energy = 0;
    }

    private void CreateArrowEffect(Vector3 position, Vector2 direction)
    {
        if (arrowEffectPrefab != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject effectInstance = Object.Instantiate(arrowEffectPrefab, position, rotation);

            effectInstance.transform.localScale *= 0.5f;
            Object.Destroy(effectInstance, 1f);
        }
       
    }
}