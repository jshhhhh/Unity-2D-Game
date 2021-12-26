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

    private PlayerStat thePlayerStat;

    public GameObject prefab_Floating_text;
    public GameObject parent;

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

    private void FloatText(int number, string color)
    {
        //floating text를 띄울 위치(좌표값)
        Vector3 vector = thePlayerStat.transform.position;
        //Player의 머리 위에 띄우기 위해
        vector.y += 60;

        //prefab clone 생성
        GameObject clone = Instantiate(prefab_Floating_text, vector, Quaternion.Euler(Vector3.zero));
        //Floating 안의 text 변수의 text 속성에 dmg를 넣어줌
        clone.GetComponent<FloatingText>().text.text = number.ToString();
        if(color == "GREEN")
            //text color를 red로
            clone.GetComponent<FloatingText>().text.color = Color.green;
        else if(color == "BLUE")
            clone.GetComponent<FloatingText>().text.color = Color.blue;
        //text의 font size를 25로
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
    }
    
    public void Useitem(int _itemID)
    {
        switch(_itemID)
        {
            case 10001:
                if(thePlayerStat.hp >= thePlayerStat.currentHP + 50)
                    thePlayerStat.currentHP += 50;
                else
                    thePlayerStat.currentHP = thePlayerStat.hp;
                FloatText(50, "GREEN");
                break;
            case 10002:
                if(thePlayerStat.mp >= thePlayerStat.currentMP + 50)
                    thePlayerStat.currentMP += 15;
                else
                    thePlayerStat.currentMP = thePlayerStat.mp;
                FloatText(50, "BLUE");
                break;
            case 10003:
                if(thePlayerStat.hp >= thePlayerStat.currentHP + 350)
                    thePlayerStat.currentHP += 350;
                else
                    thePlayerStat.currentHP = thePlayerStat.hp;
                FloatText(350, "GREEN");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();

        //아이템 정보?
        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 채워주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10002, "파란 포션", "마나를 15 채워주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10003, "농축 빨간 포션", "체력을 350 채워주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(10004, "농축 파란 포션", "마나를 80 채워주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(11001, "랜덤 상자", "랜덤으로 포션이 나온다. 낮은 확률로 꽝", Item.ItemType.Use));
        itemList.Add(new Item(20001, "짧은 검", "기본적인 용사의 검", Item.ItemType.Equip, 3));
        itemList.Add(new Item(20301, "사파이어 반지", "1초에 채력 1을 회복시켜주는 마법 반지", Item.ItemType.Equip, 0, 0, 1));
        itemList.Add(new Item(30001, "고대 유물의 조각 1", "반으로 쪼개진 고대 유물의 파편", Item.ItemType.Quest));
        itemList.Add(new Item(30002, "고대 유물의 조각 2", "반으로 쪼개진 고대 유물의 파편", Item.ItemType.Quest));
        itemList.Add(new Item(30003, "고대 유물", "고대 유적에 잠들어있던 고대의 유물", Item.ItemType.Quest));
    }
}
