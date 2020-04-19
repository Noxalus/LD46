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
            _currentActiveResource.Collect(_production);
            UI.AmountChanged(_production, _currentActiveResource.Type);
            _animator.SetTrigger("Collect");

            _audioSource.PlayOneShot(_collectSound.GetRandomSound());
        }
    }

    protected override void OnItemEnter(Item item)
    {
        base.OnItemEnter(item);

        if (_currentActiveTarget != null && _currentActiveTarget is Resource resource)
        {
            _currentActiveResource = resource;
            SetTarget(null);
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
