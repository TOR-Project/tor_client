using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomTextController : MonoBehaviour, LanguageObserever
{
    [SerializeField] internal string[] keys;
    int selectedIdx = 0;

    private void OnEnable()
    {
        selectedIdx = Random.Range(0, keys.Length);
        onLanguageChanged();
    }

    public void onLanguageChanged()
    {
        Text text = GetComponent<Text>();
        text.text = LanguageManager.instance.getText(keys[selectedIdx]);
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
