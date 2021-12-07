using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppVersionTextController : MonoBehaviour
{
    private void Awake()
    {
        Text text = GetComponent<Text>();
        text.text = "v" + Application.version;
    }
}
