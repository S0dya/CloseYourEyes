using UnityEngine;
using UnityEngine.EventSystems;

public class SaveOnPointerUp : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        SaveManager.Instance.SaveDataToFile();
    }
}
