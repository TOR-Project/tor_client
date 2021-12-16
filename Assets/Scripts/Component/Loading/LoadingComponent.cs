using UnityEngine;
using UnityEditor;

public abstract class LoadingComponent : MonoBehaviour
{
    public abstract void startLoading();
    public abstract bool isLoadingCompleted();
    public abstract string getLoadingTextKey();
}