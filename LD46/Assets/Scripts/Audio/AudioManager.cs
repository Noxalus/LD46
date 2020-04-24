using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource = null;

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
        _musicSource.clip = _music;
        _musicSource.volume = 0.35f;
        _musicSource.loop = true;
        _musicSource.Play();
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
