using System;
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
        _source.clip = _music;
        _source.volume = 0.5f;
        _source.loop = true;
        _source.Play();
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

    public void StopMusic()
    {
        _source.Stop();
    }
}
