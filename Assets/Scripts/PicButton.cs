using UnityEngine;
using UnityEngine.EventSystems;

public class PicButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        
        Debug.Log("click!");
    }
}
