using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AssetsLoadManager : MonoBehaviour
{
    static AssetsLoadManager mInstance;
    public static AssetsLoadManager instance {
        get {
            return mInstance;
        }
    }
    
    private Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();
    private Dictionary<string, AssetBundle> assetBundleMap = new Dictionary<string, AssetBundle>();
    private Dictionary<string, bool> downloadPendingMap = new Dictionary<string, bool>();

    private AssetsLoadManager()
    {
        mInstance = this;
    }

    public void requestSprite(string _url, Func<Sprite, bool> _callback, Func<float, bool> _progressCallback)
    {
        if (spriteMap.ContainsKey(_url))
        {
            _callback(spriteMap[_url]);
            _progressCallback(1);
        }
        else if (downloadPendingMap.ContainsKey(_url))
        {
            StartCoroutine(waitSprite(_url, _callback, _progressCallback));
        }
        else
        {
            downloadPendingMap[_url] = true;
            StartCoroutine(loadSprite(_url, _callback, _progressCallback));
        }
    }

    private IEnumerator waitSprite(string _url, Func<Sprite, bool> _callback, Func<float, bool> _progressCallback)
    {
        yield return new WaitUntil(() => downloadPendingMap[_url] == false);

        _callback(spriteMap[_url]);
        _progressCallback(1);
    }

    private IEnumerator loadSprite(string _url, Func<Sprite, bool> _callback, Func<float, bool> _progressCallback)
    {
        WWW www = new WWW(_url);

        if (_progressCallback != null)
        {
            StartCoroutine(DownloadProgress(www, _progressCallback));
        }

        yield return www;
        if (www.error == null)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            spriteMap[_url] = sprite;
            downloadPendingMap[_url] = false;
            _callback(sprite);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
            _callback(null);
        }
    }

    public void requestAssets(string _url, string _fileName, Func<AssetBundle, bool> _callback, Func<float, bool> _progressCallback)
    {
        if (assetBundleMap.ContainsKey(_url))
        {
            _callback(assetBundleMap[_url]);
            _progressCallback(1);
        }
        else if (downloadPendingMap.ContainsKey(_url))
        {
            StartCoroutine(waitAsset(_url, _callback, _progressCallback));
        }
        else if (File.Exists(Path.Combine(Application.streamingAssetsPath, _fileName)))
        {
            downloadPendingMap[_url] = true;
            StartCoroutine(loadAssetBundleFromStreamingFolder(_url, _fileName, _callback, _progressCallback));
        } else
        {
            downloadPendingMap[_url] = true;
            StartCoroutine(loadAsset(_url, _fileName, _callback, _progressCallback));
        }
    }

    private IEnumerator waitAsset(string _url, Func<AssetBundle, bool> _callback, Func<float, bool> _progressCallback)
    {
        yield return new WaitUntil(() => downloadPendingMap[_url] == false);

        _callback(assetBundleMap[_url]);
        _progressCallback(1);
    }

    IEnumerator loadAsset(string _url, string _fileName, Func<AssetBundle, bool> _callback, Func<float, bool> _progressCallback)
    {
        WWW www = new WWW(_url);
        if (_progressCallback != null)
        {
            StartCoroutine(DownloadProgress(www, _progressCallback));
        }
        yield return www;
        if (www.error == null)
        {
            File.WriteAllBytes(Path.Combine(Application.streamingAssetsPath, _fileName), www.bytes);
            StartCoroutine(loadAssetBundleFromStreamingFolder(_url, _fileName, _callback, _progressCallback));
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
            _callback(null);
        }
    }

    IEnumerator DownloadProgress(WWW _www, Func<float, bool> _progressCallback)
    {
        while (!_www.isDone)
        {
            _progressCallback(_www.progress);
            yield return new WaitForSeconds(0.1f);
        }
        _progressCallback(1);
    }

    IEnumerator loadAssetBundleFromStreamingFolder(string _url, string _fileName, Func<AssetBundle, bool> _callback, Func<float, bool> _progressCallback)
    {
        var fileStream = new FileStream(Path.Combine(Application.streamingAssetsPath, _fileName), FileMode.Open, FileAccess.Read);
        var bundleLoadRequest = AssetBundle.LoadFromStreamAsync(fileStream);
        yield return bundleLoadRequest;
        AssetBundle myLoadedAssetBundle = bundleLoadRequest.assetBundle;

        _progressCallback(1);
        assetBundleMap[_url] = myLoadedAssetBundle;
        downloadPendingMap[_url] = false;
        _callback(myLoadedAssetBundle);
    }
}
