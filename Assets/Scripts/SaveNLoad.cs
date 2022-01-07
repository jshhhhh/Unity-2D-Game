using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//인풋, 아웃풋
using System.IO;
//직렬화 된 것을 바이너리 파일로 만들기 위핸 포매터
using System.Runtime.Serialization.Formatters.Binary;
//씬 전환
using UnityEngine.SceneManagement;

public class SaveNLoad : MonoBehaviour
{
    //직렬화?: 컴퓨터가 읽기 쉽게 바꿈
    //직렬화는 자료형만 가능
    [System.Serializable]
    public class Data
    {
        //Vector3는 직렬화가 불가능하므로 나눠서
        public float playerX;
        public float playerY;
        public float playerZ;

        public int playerLv;
        public int playerHP;
        public int playerMP;

        public int playerCurrentHP;
        public int playerCurrentMP;
        public int playerCurrentEXP;

        public int playerHPR;
        public int playerMPR;

        public int playerATK;
        public int playerDEF;

        public int added_atk;
        public int added_def;
        public int added_hpr;
        public int added_mpr;

        //아이템 ID, 아이템 개수
        public List<int> playerItemInventory;
        public List<int> playerItemInventoryCount;
        //장착한 아이템의 ID값
        public List<int> playerEquipItem;

        public string mapName;
        public string sceneName;

        //스위치 리스트
        public List<bool> swList;
        public List<string> swNameList;
        //변수 리스트
        public List<string> varNameList;
        public List<float> varNumberList;
    }

    private PlayerManager thePlayer;
    private PlayerStat thePlayerStat;
    private DatabaseManager theDatabase;
    private Inventory theInven;
    private Equipment theEquip;
    private FadeManager theFade;

    public Data data;

    private Vector3 vector;

    public void CallSave()
    {
        theDatabase = FindObjectOfType<DatabaseManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theEquip = FindObjectOfType<Equipment>();
        theInven = FindObjectOfType<Inventory>();

        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;

        data.playerLv = thePlayerStat.character_Lv;
        data.playerHP = thePlayerStat.hp;
        data.playerMP = thePlayerStat.mp;
        data.playerCurrentHP = thePlayerStat.currentHP;
        data.playerCurrentMP = thePlayerStat.currentMP;
        data.playerCurrentEXP = thePlayerStat.currentEXP;
        data.playerATK = thePlayerStat.atk;
        data.playerDEF = thePlayerStat.def;
        data.playerHPR = thePlayerStat.recover_hp;
        data.playerMPR = thePlayerStat.recover_mp;
        data.added_atk = theEquip.added_atk;
        data.added_def = theEquip.added_def;
        data.added_hpr = theEquip.added_hpr;
        data.added_mpr = theEquip.added_mpr;

        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;

        Debug.Log("기초 데이터 입력 성공");

        //기존의 데이터를 지우고 추가함
        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();
        data.playerEquipItem.Clear();

        //데이터베이스의 모든 변수들을 List에 추가
        for (int i = 0; i < theDatabase.var_name.Length; i++)
        {
            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumberList.Add(theDatabase.var[i]);
        }

        //데이터베이스의 모든 스위치들을 List에 추가
        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {
            data.swNameList.Add(theDatabase.switch_name[i]);
            data.swList.Add(theDatabase.switches[i]);
        }

        //임시로 itemList를 만들어 inventoryItemList에 함수로 접근
        List<Item> itemList = theInven.SaveItem();

        for (int i = 0; i < itemList.Count; i++)
        {
            //인벤토리의 ItemID와 개수 값이 저장됨
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
            Debug.Log("인벤토리의 아이템 저장 완료 : " + itemList[i].itemID);
        }

        for (int i = 0; i < theEquip.equipItemList.Length; i++)
        {
            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
            Debug.Log("장착된 아이템 저장 완료" + theEquip.equipItemList[i].itemID);
        }

        //변환하는 포매터 생성과 선언
        BinaryFormatter bf = new BinaryFormatter();
        //파일 입출력기(메모장)
        //Application.dataPath: 프로젝트 폴더 경로
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");

        //변환기로 직렬화시킴
        //data 클래스의 정보들을 file에 기록하고, 직렬화시킴
        bf.Serialize(file, data);
        file.Close();

        Debug.Log(Application.dataPath + "의 위치에 저장했습니다.");
    }

    public void CallLoad()
    {
        //변환하는 포매터 생성과 선언
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

        //파일이 있고, 한 글자라도 있을 경우
        if (file != null && file.Length > 0)
        {
            //파일을 원래대로 바꿈(Data 형식으로 명시적 변환)
            data = (Data)bf.Deserialize(file);

            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerManager>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theEquip = FindObjectOfType<Equipment>();
            theInven = FindObjectOfType<Inventory>();
            theFade = FindObjectOfType<FadeManager>();

            theFade.FadeOut();

            //data에 저장된 정보들을 다시 불러옴
            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            thePlayerStat.character_Lv = data.playerLv;
            thePlayerStat.hp = data.playerHP;
            thePlayerStat.mp = data.playerMP;
            thePlayerStat.currentHP = data.playerCurrentHP;
            thePlayerStat.currentMP = data.playerCurrentMP;
            thePlayerStat.currentEXP = data.playerCurrentEXP;
            thePlayerStat.atk = data.playerATK;
            thePlayerStat.def = data.playerDEF;
            thePlayerStat.recover_hp = data.playerHPR;
            thePlayerStat.recover_mp = data.playerMPR;

            theEquip.added_atk = data.added_atk;
            theEquip.added_def = data.added_def;
            theEquip.added_hpr = data.added_hpr;
            theEquip.added_mpr = data.added_mpr;

            //리스트를 Arra로 변환, 배열에 쉽고 간단하게 입력 가능
            theDatabase.var = data.varNumberList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();

            //장착된 아이템과
            for (int i = 0; i < theEquip.equipItemList.Length; i++)
            {
                //데이터베이스의 아이템이 일치하면
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerEquipItem[i] == theDatabase.itemList[x].itemID)
                    {
                        //그 아이템을 등록시킴
                        theEquip.equipItemList[i] = theDatabase.itemList[x];
                        Debug.Log("장착된 아이템을 로드했습니다 : " + theEquip.equipItemList[i].itemID);
                        break;
                    }
                }
            }

            List<Item> itemList = new List<Item>();

            //저장된 인벤토리의 개수만큼 반복
            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    //소지한(저장된) 아이템의 ID값이 데이터베이트에 있는 아이템값과 똑같다면 임시 아이템 리스트에 통째로 추가
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("인벤토리 아이템을 로드했습니다 : " + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }

            //개수 추가
            for (int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }

            //저장된 인벤토리 불러옴
            theInven.LoadItem(itemList);
            theEquip.ShowTxT();

            StartCoroutine(WaitCoroutine());
        }
        else
        {
            Debug.Log("저장된 세이브 파일이 없습니다.");
        }

        //열었던 파일 닫음
        file.Close();
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(2.5f);

        //로드 후 실행됨(코루틴)
        GameManager theGM = FindObjectOfType<GameManager>();
        theGM.LoadStart();

        //씬 로드
        SceneManager.LoadScene(data.sceneName);
    }
}
