using UnityEngine;

public class ShieldBash : PlayerSkill
{
    private GameObject shieldBashEffectPrefab;

    public ShieldBash() : base(
        "방패 가격",
        "적을 때려 기본 피해+공격 피해*스킬 계수 의 물리적 피해를 입힙니다.",
        5,
        new int[] { 10, 20, 30, 40, 50 },
        new float[] { 1.15f, 1.3f, 1.45f, 1.6f, 1.75f }
    )
    {
    }

    public void SetEffectPrefab(GameObject prefab)
    {
        shieldBashEffectPrefab = prefab;
    }

    public override void UseSkill(Hero caster)
    {
        Knight knight = caster as Knight;
        if (knight == null)
        {
            Debug.LogError("ShieldBash skill can only be used by Knight");
            return;
        }
        if (knight._target == null)
        {
            Debug.Log("No target found for ShieldBash skill");
            return;
        }

        int baseDamage = BaseDamage[Level - 1];
        float coefficient = Coefficients[Level - 1];
        float totalDamage = baseDamage + (knight.attackDamage * coefficient);

        Enemy enemy = knight._target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(knight, totalDamage);
            Debug.Log($"ShieldBash hit {enemy.name} for {totalDamage} damage");

            // 스킬 이펙트 생성
            CreateSkillEffect(enemy.transform.position, knight.transform.position);
        }
        else
        {
            Debug.Log("ShieldBash target is not an enemy");
        }

        // 스킬 사용 후 에너지 소모
        knight.Energy = 0;
    }

    private void CreateSkillEffect(Vector3 targetPosition, Vector3 knightPosition)
    {
        if (shieldBashEffectPrefab != null)
        {
            // 방향 계산
            Vector2 direction = (targetPosition - knightPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // 이펙트 생성
            GameObject effectInstance = Object.Instantiate(shieldBashEffectPrefab, targetPosition, rotation);

            // 이펙트 크기를 더 크게 조정 (예: 원래 크기의 2배)
            effectInstance.transform.localScale *= 2f;

            // 이펙트 지속 시간을 약간 늘림 (선택적)
            Object.Destroy(effectInstance, 1.5f);
        }
        else
        {
            Debug.LogWarning("Shield Bash effect prefab is not assigned!");
        }
    }
}