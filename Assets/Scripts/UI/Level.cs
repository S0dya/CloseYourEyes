using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject locked;
    [SerializeField] GameObject unLocked;
    [SerializeField] GameObject rune;

    public void SetRune()
    {
        locked.SetActive(false);
        rune.SetActive(true);
    }

    public void UnLock()
    {
        locked.SetActive(false);
        unLocked.SetActive(true);
    }
}
