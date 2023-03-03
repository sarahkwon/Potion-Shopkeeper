using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Transform tooltipUI;

    public PlayerInventory playerInventory;

    [SerializeField] private Vector2 tooltipOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;


        tooltipUI.GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);
        tooltipUI.position = position;



    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject icon = eventData.pointerEnter.gameObject;

        Item item = playerInventory.GetItemFromIcon(icon);

        if (item == null)
        {
            return;
        }

        string itemName = item.name;
        string itemDesc = item.description;

        tooltipUI.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemName;
        tooltipUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemDesc;

        tooltipUI.gameObject.SetActive(true);

    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltipUI.gameObject.SetActive(false);
    }

}
