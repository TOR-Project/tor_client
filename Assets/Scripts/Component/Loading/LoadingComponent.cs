using UnityEngine;
using UnityEditor;

public abstract class LoadingComponent : MonoBehaviour
{
    public abstract void startLoading();
    public abstract string getLoadingTextKey();
    public virtual int getProgressMax()
    {
        return 1;
    }

    public abstract int getProgressCurrent();

    public bool isLoadingCompleted()
    {
        return getProgressMax() <= getProgressCurrent();
    }

    public virtual string getLoadingText()
    {
        return LanguageManager.instance.getText(getLoadingTextKey());
    }
}