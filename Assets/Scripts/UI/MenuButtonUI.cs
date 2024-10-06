using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverSprite;

    private void OnEnable()
    {
        hoverSprite.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverSprite.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverSprite.SetActive(false);
    }
}