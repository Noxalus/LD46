public class Soldier : Unit
{
    protected override void ExecuteAction()
    {
        base.ExecuteAction();

        if (_currentActiveTarget != null)
        {
            Attack();
        }
        else
        {
            // Search for new target
            FindNewTarget();
        }
    }

    protected override void OnItemEnter(Item item)
    {
        base.OnItemEnter(item);

        if (item.tag == "Enemy")
        {
            _currentActiveTarget = item;
        }
    }
}
