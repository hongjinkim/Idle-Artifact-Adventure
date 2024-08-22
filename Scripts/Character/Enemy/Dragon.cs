public class Dragon : Enemy
{
    public string Stage;

    protected override void OnAnimAttack()
    {
        animator.SetTrigger("Slash1H");
        IsAction = true;
    }

}
