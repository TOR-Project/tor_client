using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class PedtClearCaching
{

    [MenuItem("Util/CleanCache")]
    public static void CleanCache()
    {
        Caching.ClearCache();

        AssetsLoadManager.instance.cleanAll();
        LanguageManager.instance.cleanAll();

        List<GameObject> rootObjects = new List<GameObject>();
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);
        Debug.Log("Root object = " + rootObjects.Count);

        int count = 0;
        foreach (GameObject go in rootObjects) {
            count += ResetObject(go);
        }

        System.GC.Collect();
        EditorUtility.DisplayDialog("알림", "캐시가 삭제되었습니다.(" + count + ")", "확인");
    }

    public static int ResetObject(GameObject _ob)
    {
        int count = 0;

        for (int i = 0; i < _ob.transform.childCount; i++)
        {
            count += ResetObject(_ob.transform.GetChild(i).gameObject);
        }

        ImageLoadingComponent ilc = _ob.GetComponent<ImageLoadingComponent>();
        if (ilc != null)
        {
            ilc.resetAll();
            count++;
        }

        ButtonPressedSpriteLoadingComponent bpslc = _ob.GetComponent<ButtonPressedSpriteLoadingComponent>();
        if (bpslc != null)
        {
            bpslc.resetAll();
            count++;
        }

        SpriteLoadingComponent slc = _ob.GetComponent<SpriteLoadingComponent>();
        if (slc != null)
        {
            slc.resetAll();
            count++;
        }

        LanguageTextController ltc = _ob.GetComponent<LanguageTextController>();
        if (ltc != null)
        {
            ltc.resetAll();
            count++;
        }

        return count;
    }
}