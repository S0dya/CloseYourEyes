using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EyeButton : SingletonMonobehaviour<EyeButton>, IPointerDownHandler, IPointerUpHandler
{
    Animator eye;

    protected override void Awake()
    {
        base.Awake();

        eye = GetComponentInChildren<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Player.Instance.OpenEye();
        eye.Play("OpenEye");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Player.Instance.CloseEye();
        eye.Play("CloseEye");
    }
}