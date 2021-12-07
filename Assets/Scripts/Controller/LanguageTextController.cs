using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageTextController : MonoBehaviour, LanguageObserever
{
    [SerializeField] string key;

    public void onLanguageChanged()
    {
        Text text = GetComponent<Text>();
        text.text = LanguageManager.instance.getText(key);
    }

    private void Awake()
    {
        LanguageManager.instance.addObserver(this);
    }

    private void OnDestroy()
    {
        LanguageManager.instance.removeObserver(this);
    }
}
