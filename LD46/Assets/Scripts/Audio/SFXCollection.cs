using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXCollection.asset", menuName ="LD46/SFXCollection")]
public class SFXCollection : ScriptableObject
{
    [SerializeField]
    private List<AudioClip> _sounds = new List<AudioClip>();

    public AudioClip GetRandomSound()
    {
        return _sounds[Random.Range(0, _sounds.Count)];
    }
}
