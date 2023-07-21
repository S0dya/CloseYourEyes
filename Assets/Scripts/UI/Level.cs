using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject locked;
    [SerializeField] GameObject unLocked;
    [SerializeField] GameObject rune;

    public void Lock()
    {
        locked.SetActive(true);
    }
    
    public void SetRune()
    {
        rune.SetActive(true);
    }

    public void UnLock()
    {
        unLocked.SetActive(true);
    }
}
