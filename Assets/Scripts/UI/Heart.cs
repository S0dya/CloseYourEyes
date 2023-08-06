using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    Image image;
    [SerializeField] Sprite heart;
    [SerializeField] Sprite rottedHeart;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void ToggleHeart(bool val)
    {
        if (val)
        {
            image.sprite = rottedHeart;
        }
        else
        {
            image.sprite = heart;
        }
    }
}
