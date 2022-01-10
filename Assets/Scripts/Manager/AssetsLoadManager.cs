using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
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

    public void cleanAll()
    {
        spriteMap.Clear();
        assetBundleMap.Clear();
        downloadPendingMap.Clear();
    }

    public void requestSprite(string _url, Func<Sprite, bool> _callback, Func<float, bool> _progressCallback)
    {
        if (spriteMap.ContainsKey(_url))
        {
            _callback(spriteMap[_url]);
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

    public void requestAssets(string _fileName, Func<AssetBundle, bool> _callback, Func<float, bool> _progressCallback)
    {
        if (assetBundleMap.ContainsKey(_fileName))
        {
            _callback(assetBundleMap[_fileName]);
        }
        else if (downloadPendingMap.ContainsKey(_fileName))
        {
            StartCoroutine(waitAsset(_fileName, _callback, _progressCallback));
        }
        else
        {
            downloadPendingMap[_fileName] = true;
            StartCoroutine(loadAsset(_fileName, _callback, _progressCallback));
        }
    }

    private IEnumerator waitAsset(string _fileName, Func<AssetBundle, bool> _callback, Func<float, bool> _progressCallback)
    {
        yield return new WaitUntil(() => downloadPendingMap[_fileName] == false);

        _callback(assetBundleMap[_fileName]);
    }

    IEnumerator loadAsset(string _fileName, Func<AssetBundle, bool> _callback, Func<float, bool> _progressCallback)
    {
        AssetBundle myLoadedAssetBundle = null;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(Path.Combine(Application.streamingAssetsPath, _fileName));
            yield return unityWebRequest.SendWebRequest();
            Debug.Log("responseCode:" + unityWebRequest.responseCode);
            if (unityWebRequest.isDone)
            {
                myLoadedAssetBundle = (unityWebRequest.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            }
        }
        else
        {
            myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, _fileName));
        }

        if (myLoadedAssetBundle != null)
        {
            assetBundleMap[_fileName] = myLoadedAssetBundle;
            downloadPendingMap[_fileName] = false;
            _callback(myLoadedAssetBundle);
        }
        else
        {
            _callback(null);
        }

        yield return null;
    }

    IEnumerator DownloadProgress(WWW _www, Func<float, bool> _progressCallback)
    {
        while (!_www.isDone)
        {
            _progressCallback(_www.progress);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
