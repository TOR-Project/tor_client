using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool isSoundOn;
    public AudioClip currentBgm;

    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource sePlayer;

    Dictionary<string, AudioClip> bgmAudioMap = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> seAudioMap = new Dictionary<string, AudioClip>();
    private Dictionary<string, bool> downloadPendingMap = new Dictionary<string, bool>();

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
        // StartCoroutine(loadSound("login", "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/sound/connect+wallet/bgm/connect+wallet.wav", bgmAudioMap, () => playBgm("login")));
        StartCoroutine(loadSound("wallet", "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/sound/connect+wallet/effect/button.wav", seAudioMap, null));
    }

    public void playBgm(string _key)
    {
        if (!bgmAudioMap.ContainsKey(_key))
        {
            bgmPlayer.Stop();
            return;
        }

        if (currentBgm != bgmAudioMap[_key])
        {
            currentBgm = bgmAudioMap[_key];

            playBgmCurrent();
        }
    }

    private void playBgmCurrent()
    {
        if (!isSoundOn)
        {
            bgmPlayer.Stop();
            return;
        }

        if (currentBgm != null)
        {
            bgmPlayer.clip = currentBgm;
            bgmPlayer.Play();
        }
    }

    public void stopSeNow()
    {
        sePlayer.Stop();
    }

    public void toggleSoundOn()
    {
        isSoundOn = !isSoundOn;
        playBgmCurrent();
    }

    public void playSoundEffect(string _key)
    {
        if (!isSoundOn)
        {
            sePlayer.Stop();
            return;
        }

        if (!seAudioMap.ContainsKey(_key))
        {
            return;
        }

        sePlayer.clip = seAudioMap[_key];
        sePlayer.Play();
    }

    public void requestSound(string _key, string _url, bool _isBgm, Action _callback)
    {
        Dictionary<string, AudioClip> map = _isBgm ? bgmAudioMap : seAudioMap;

        if (map.ContainsKey(_key))
        {
            _callback();
        }
        else if (downloadPendingMap.ContainsKey(_url))
        {
            StartCoroutine(waitDownloadSound(_url, _callback));
        }
        else
        {
            downloadPendingMap[_url] = true;
            StartCoroutine(loadSound(_key, _url, map, _callback));
        }
    }

    private IEnumerator waitDownloadSound(string _url, Action _callback)
    {
        yield return new WaitUntil(() => downloadPendingMap[_url] == false);

        _callback();
    }

    private IEnumerator loadSound(string _key, string _url, Dictionary<string, AudioClip> _map, Action _callback)
    {
        WWW www = new WWW(_url);
        yield return www;
        if (www.error == null)
        {
            _map.Add(_key, www.GetAudioClip());
            downloadPendingMap[_url] = false;
            _callback?.Invoke();
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
            _callback?.Invoke();
        }
    }
}
