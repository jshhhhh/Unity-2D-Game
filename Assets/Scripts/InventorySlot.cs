using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text itemName_Text;
    public Text itemCount_Text;
    public GameObject selected_Item;

    public void Additem(Item _item)
    {
        itemName_Text.text = _item.itemName;
        //Source Image를 바꾸기 위해 sprite 속성이 필요
        icon.sprite = _item.itemIcon;
        //아이템 타입이 소모품일 때만 숫자(개수) 표시
        if(Item.ItemType.Use == _item.itemType)
        {
            if(_item.itemCount > 0)
                itemCount_Text.text = "x " + _item.itemCount.ToString();
            else
                itemCount_Text.text = "";
        }
    }

    //아이템을 없애는 함수
    public void RemoveItem()
    {
        //초기화시킴
        itemName_Text.text = "";
        itemCount_Text.text = "";
        icon.sprite = null;
    }
}
