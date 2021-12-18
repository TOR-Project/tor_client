using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsLoadManager : MonoBehaviour
{
    static AssetsLoadManager mInstance;
    public static AssetsLoadManager instance {
        get {
            return mInstance;
        }
    }
    
    private Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();
    private Dictionary<string, bool> downloadPendingMap = new Dictionary<string, bool>();

    private AssetsLoadManager()
    {
        mInstance = this;
    }

    public void requestSprite(string url, Func<Sprite, bool> callback)
    {
        if (spriteMap.ContainsKey(url))
        {
            callback(spriteMap[url]);
        }
        else if (downloadPendingMap.ContainsKey(url))
        {
            StartCoroutine(waitSprite(url, callback));
        }
        else
        {
            downloadPendingMap[url] = true;
            StartCoroutine(loadSprite(url, callback));
        }
    }

    private IEnumerator waitSprite(string url, Func<Sprite, bool> callback)
    {
        yield return new WaitUntil(() => downloadPendingMap[url] == false);

        callback(spriteMap[url]);
    }

    private IEnumerator loadSprite(string url, Func<Sprite, bool> callback)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            spriteMap[url] = sprite;
            downloadPendingMap[url] = false;
            callback(sprite);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
            callback(null);
        }
    }
}
