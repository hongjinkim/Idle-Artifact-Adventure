public interface IBehavior
{
    void Move();
    void Attack();
    void TakeDamage(Character target, float _damage);
    void Die();
    bool Alive();
}
