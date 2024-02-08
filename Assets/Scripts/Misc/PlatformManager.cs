using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlatformManager : MonoBehaviour
{
    [SerializeField] GameObject MobileUIInput;

    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        Toggle(true);
#else
        Toggle(false);
#endif
    }

    void Toggle(bool toggle)
    {
        MobileUIInput.SetActive(toggle);
    }
}
