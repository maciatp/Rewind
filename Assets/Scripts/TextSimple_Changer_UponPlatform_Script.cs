using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSimple_Changer_UponPlatform_Script : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro text;
    [SerializeField] string newText;

#if UNITY_IOS || UNITY_ANDROID

    private void Awake()
    {
        text = gameObject.GetComponent<TMPro.TextMeshPro>();
    }
    private void Start()
    {
        text.text = newText;
    }


#endif
}
