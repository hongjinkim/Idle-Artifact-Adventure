using UnityEngine;
using System.Collections.Generic;

public class ScorchedEarth : PlayerSkill
{
    public float aoeRadius = 0.8f; // AOE 범위 반경
    private float damageMultiplier = 1f;
    private GameObject scorchedEarthEffectPrefab;

    public ScorchedEarth() : base(
        "그을린 대지",
        "마법사는 대지를 태우는 불꽃을 소환하여 기본 피해+공격피해*스킬계수의 범위 마법피해를 입힙니다.",
        5,
        new int[] { 15, 25, 35, 45, 60 },
        new float[] { 1.2f, 1.4f, 1.6f, 1.8f, 2.0f }
    )
    {
    }

    public void SetEffectPrefab(GameObject prefab)
    {
        scorchedEarthEffectPrefab = prefab;
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public override void UseSkill(Hero caster)
    {
        Wizard wizard = caster as Wizard;
        if (wizard == null)
        {
            Debug.LogError("Caster is not a Wizard!");
            return;
        }
        if (wizard._target == null)
        {
            Debug.LogWarning("No target found for Scorched Earth skill.");
            return;
        }

        int baseDamage = BaseDamage[Level - 1];
        float coefficient = Coefficients[Level - 1];
        float totalDamage = (baseDamage + (wizard.attackDamage * coefficient)) * damageMultiplier;

        Vector2 targetPosition = wizard._target.transform.position;

        // 범위 내의 모든 적을 찾아 데미지를 입힘
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPosition, aoeRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(wizard, totalDamage);
                Debug.Log($"ScorchedEarth hit {enemy.name} for {totalDamage} damage");
            }
        }

        // 스킬 사용 후 에너지 소모
        wizard.Energy = 0;

        // 스킬 이펙트 생성
        CreateSkillEffect(targetPosition);
    }

    private void CreateSkillEffect(Vector2 position)
    {
        if (scorchedEarthEffectPrefab != null)
        {
            GameObject effectInstance = Object.Instantiate(scorchedEarthEffectPrefab, position, Quaternion.identity);

            // 이펙트의 크기를 AOE 반경에 맞게 조정
            float effectScale = aoeRadius * 2; // 직경을 기준으로 크기 조정
            effectInstance.transform.localScale = new Vector3(effectScale, effectScale, effectScale);

            // 이펙트 지속 시간 설정 (예: 2초)
            Object.Destroy(effectInstance, 1f);
        }
        else
        {
            Debug.LogWarning("Scorched Earth effect prefab is not assigned!");
        }
    }
}