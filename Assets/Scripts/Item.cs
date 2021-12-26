using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    //아이템의 고유 ID값. 중복 불가능
    public int itemID;
    //아이템의 이름. 중복 가능
    public string itemName;
    //아이템 설명
    public string itemDescription;
    //소지 갯수
    public int itemCount;
    //아이템의 아이콘
    public Sprite itemIcon;
    //아이템타입
    public ItemType itemType;

    //타입 열거(이외의 값을 벗어난 값을 대입할 경우 오류)
    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }

    //예시로 몇 개의 능력치만
    public int atk;
    public int def;
    public int recover_hp;
    public int recover_mp;

    //생성자(호출되는 순간 생성자를 통해 값을 채움)
    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _atk = 0, int _def = 0, int _recover_hp = 0, int _recover_mp = 0, int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        //Sprite를 Resource의 해당 경로의 파일을 로드
        //물음표 / 사용 필수(키패드 / 쓰면 오류 남)
        //typeof(Sprite): Sprite로 가져옴(명시적 변환)
        //as Sprite: 실제로 변환
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;

        atk = _atk;
        def = _def;
        recover_hp = _recover_hp;
        recover_mp = _recover_mp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
