using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EyeButton : SingletonMonobehaviour<EyeButton>, IPointerDownHandler, IPointerUpHandler
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Player.Instance.OpenEye();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Player.Instance.CloseEye();
    }
}