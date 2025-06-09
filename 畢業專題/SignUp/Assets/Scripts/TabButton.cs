using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Tabs tabManager;
    public Image background;

    void Start()
    {
        tabManager.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabManager.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabManager.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabManager.OnTabExit(this);
    }
}
