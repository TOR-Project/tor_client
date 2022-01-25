using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class LoadingComponentPool : MonoBehaviour
{
    List<LoadingComponent> loadingComponents = null;

    public void init()
    {
        if (loadingComponents == null)
        {
            loadingComponents = new List<LoadingComponent>();
            addAllLoadingComponent(gameObject);
        }
    }

    private void addAllLoadingComponent(GameObject _ob)
    {
        for (int i = 0; i < _ob.transform.childCount; i++)
        {
            addAllLoadingComponent(_ob.transform.GetChild(i).gameObject);
        }

        LoadingComponent[] lcList = _ob.GetComponents<LoadingComponent>();
        if (lcList != null)
        {
            foreach (LoadingComponent lc in lcList)
            {
                loadingComponents.Add(lc);
            }
        }
    }

    public LoadingComponent[] getLoadingComponents()
    {
        return loadingComponents.ToArray();
    }

    public int getSize()
    {
        return loadingComponents.Count;
    }

    public bool isAllLoadingCompleted()
    {
        foreach (LoadingComponent lc in loadingComponents)
        {
            if (!lc.isLoadingCompleted())
            {
                return false;
            }
        }

        return true;
    }
}