using UnityEngine;
using UnityEditor;

public class LoadingComponentPool : MonoBehaviour
{
    [SerializeField]
    LoadingComponent[] loadingComponents;

    public LoadingComponent[] getLoadingComponents()
    {
        return loadingComponents;
    }

    public int getSize()
    {
        if (loadingComponents == null)
        {
            return 0;
        }
        return loadingComponents.Length;
    }

    public bool isAllLoadingCompleted()
    {
        if (loadingComponents == null)
        {
            return true;
        }

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