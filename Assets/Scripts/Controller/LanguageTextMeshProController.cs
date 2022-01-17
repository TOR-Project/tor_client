using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LanguageTextMeshProController : MonoBehaviour, LanguageObserever
{
    [SerializeField] internal string key;

    public void onLanguageChanged()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
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

    public void resetAll()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = "";
    }
}
