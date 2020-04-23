using UnityEngine;

public class Worker : Unit
{
    [SerializeField]
    private int _production = 1;

    [SerializeField]
    private SFXCollection _collectSound = null;


    private Resource _currentActiveResource;

    protected override void ExecuteAction()
    {
        base.ExecuteAction();

        if (_currentActiveResource != null)
        {
            transform.LookAt(_currentActiveResource.transform);
            _currentActiveResource.Collect(_production);
            UI.AmountChanged(_production, _currentActiveResource.Type);
            _animator.SetTrigger("Collect");

            _audioSource.PlayOneShot(_collectSound.GetRandomSound());
        }
    }

    public override void SetActiveTarget(Item target)
    {
        base.SetActiveTarget(target);

        CheckResourceTarget();
    }

    protected override void OnItemEnter(Item item)
    {
        base.OnItemEnter(item);

        CheckResourceTarget();
    }

    private void CheckResourceTarget()
    {
        if (_currentActiveTarget != null && _currentActiveTarget is Resource resource)
        {
            _currentActiveResource = resource;
            SetLocationTarget(null);
        }
    }

    protected override void OnItemExit(Item item)
    {
        if (item == _currentActiveResource)
        {
            _currentActiveResource = null;
        }

        base.OnItemExit(item);
    }
}
