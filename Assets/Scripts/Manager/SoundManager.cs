using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public const int BGM_NORMAL = 0;
    public const int BGM_BOSS = 1;

    public const int SE_EAT = 0;

    public bool isSoundOn = false;
    public int bossPhaseCount = 0;
    public AudioClip currentBgm;

    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource sePlayer;

    [SerializeField] AudioClip[] bgmAudio;
    [SerializeField] AudioClip[] seAudio;

    private bool isLoadingCompleted = false;

    static SoundManager mInstance;
    public static SoundManager instance {
        get {
            return mInstance;
        }
    }

    private SoundManager()
    {
        mInstance = this;
    }

    private void Awake()
    {
        StartCoroutine(loadSound());
    }

    private void playBgm()
    {
        if (!isSoundOn)
        {
            currentBgm = null;
            bgmPlayer.Stop();
            return;
        }

        AudioClip ac;
        if (bossPhaseCount <= 0)
        {
            ac = bgmAudio[BGM_NORMAL];
        }
        else
        {
            ac = bgmAudio[BGM_BOSS];
        }

        if (ac != null && currentBgm != ac)
        {
            currentBgm = ac;
            bgmPlayer.clip = ac;
            bgmPlayer.Play();
        }
    }

    private IEnumerator loadSound()
    {
        isSoundOn = true;
        playBgm();

        isLoadingCompleted = true;

        yield return null;
    }

    public bool isReady()
    {
        return isLoadingCompleted;
    }

    public void setSound(bool isOn)
    {
        isSoundOn = isOn;
        playBgm();
    }

    public AudioClip getSoundEffect(int idx)
    {
        if (seAudio == null || seAudio.Length <= idx)
        {
            return null;
        }

        return seAudio[idx];
    }

    public void playSoundEffect(int idx)
    {
        if (seAudio == null || seAudio.Length <= idx || !isSoundOn)
        {
            return;
        }

        sePlayer.clip = seAudio[idx];
        sePlayer.Play();
    }

    public void setBossPhase(bool set)
    {
        bossPhaseCount += (set ? 1 : -1);

        playBgm();
    }
}
