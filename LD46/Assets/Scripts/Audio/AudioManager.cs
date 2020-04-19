using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source = null;

    [SerializeField]
    private AudioClip _music = null;

    [SerializeField]
    private SFXCollection _selectUnitSounds = null;

    [SerializeField]
    private SFXCollection _executeOrderSounds = null;

    [SerializeField]
    private AudioClip _placeItemSound = null;

    public void PlayMusic()
    {
        // TODO
    }

    public void PlayPlaceItemSound()
    {
        _source.PlayOneShot(_placeItemSound);
    }

    public void PlaySelectUnitSound()
    {
        _source.PlayOneShot(_selectUnitSounds.GetRandomSound());
    }

    public void PlayExecuteOrderSound()
    {
        _source.PlayOneShot(_executeOrderSounds.GetRandomSound());
    }
}
