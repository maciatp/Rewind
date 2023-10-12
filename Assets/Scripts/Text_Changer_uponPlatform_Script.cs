using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Changer_uponPlatform_Script : MonoBehaviour
{

    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] string newText;

#if UNITY_IOS || UNITY_ANDROID

    private void Awake()
    {
        text = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void Start()
    {
        text.text = newText;
    }


#endif
}
