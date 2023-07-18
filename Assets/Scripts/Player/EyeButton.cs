using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EyeButton : SingletonMonobehaviour<EyeButton>, IPointerDownHandler, IPointerUpHandler
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

    public void OnPointerUp(PointerEventData eventData)
    {
        Player.Instance.CloseEye();
    }
}