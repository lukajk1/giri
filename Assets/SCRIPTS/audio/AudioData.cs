using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioData {
    public ADFM.Sfx _Sfx;
    public AudioClip _AudioClip;
    public AudioSource _AudioSource;

    public AudioData(ADFM.Sfx sfx, AudioClip clip, AudioSource source) {
        this._Sfx = sfx;
        this._AudioClip = clip;
        this._AudioSource = source;
    }
}
