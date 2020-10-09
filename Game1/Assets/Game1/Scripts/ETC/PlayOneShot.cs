using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShot : MonoBehaviour
{
    public SfxType sfxType;

    public void PlaySFX ()
    {
        SoundManager.PlaySFX(sfxType);
    }
}
