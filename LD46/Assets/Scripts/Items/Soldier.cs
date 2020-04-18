public class Soldier : Unit
{
    protected override void ExecuteAction()
    {
        base.ExecuteAction();

        if (_currentActiveTarget != null)
        {
            _currentActiveTarget.TakeDamage(_attack);
        }
        else
        {
            // Search for new target
            FindNewTarget();
        }
    }
}
