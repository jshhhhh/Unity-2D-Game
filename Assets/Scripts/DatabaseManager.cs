using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    /*
    데이터베이스가 필요한 이유
    1. 지난 씬의 이벤트를 재발동하고 싶지 않지만 씬을 넘어가면 객체가 파괴되고 재생성되기 때문에 지난 씬을 재방문하면 변경된 내용이 전부 초기화됨(이벤트 재발동됨). 그래서 데이터베이스 스크립트를 일종의 전역 변수로 활용하여 위 현상을 방지
    2. 세이브와 로드를 기록
    3. 아이템(이름, id값, 설명)을 데이터베이스에 기술해놓고 가져다 쓰기만 하면 편함    
    */

    static public DatabaseManager instance;

    #region Singleton
    private void Awake() {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
        //이 오브젝트를 다른 씬을 불러올 때마다 파괴시키지 말라는 명령어
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    #endregion Singleton

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;

    //아이템 리스트 생성
    public List<Item> itemList = new List<Item>();
    
    public void Useitem(int _itemID)
    {
        switch(_itemID)
        {
            case 10001:
                Debug.Log("Hp가 50 회복되었습니다.");
                //thePlayer.hp += 50;
                break;
            case 10002:
                Debug.Log("Mp가 50 회복되었습니다.");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //아이템 정보?
        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 채워주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10002, "파란 포션", "마나를 15 채워주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10003, "농축 빨간 포션", "체력을 350 채워주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(10004, "농축 파란 포션", "마나를 80 채워주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(11001, "랜덤 상자", "랜덤으로 포션이 나온다. 낮은 확률로 꽝", Item.ItemType.Use));
        itemList.Add(new Item(20001, "짧은 검", "기본적인 용사의 검", Item.ItemType.Equip));
        itemList.Add(new Item(21001, "사파이어 반지", "1분에 마나 1을 회복시켜주는 마법 반지", Item.ItemType.Equip));
        itemList.Add(new Item(30001, "고대 유물의 조각 1", "반으로 쪼개진 고대 유물의 파편", Item.ItemType.Quest));
        itemList.Add(new Item(30002, "고대 유물의 조각 2", "반으로 쪼개진 고대 유물의 파편", Item.ItemType.Quest));
        itemList.Add(new Item(30003, "고대 유물", "고대 유적에 잠들어있던 고대의 유물", Item.ItemType.Quest));
    }
}
