using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EyeButton : SingletonMonobehaviour<EyeButton>, IPointerDownHandler, IPointerExitHandler
{
    [HideInInspector] public Animator eye;

    protected override void Awake()
    {
        base.Awake();

        eye = GetComponentInChildren<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Player.Instance.canOpenEye)
        {
            Player.Instance.OpenEye();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Player.Instance.CloseEye();
    }
}