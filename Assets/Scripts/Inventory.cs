using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static public Inventory instance;


    private DatabaseManager theDatabase;
    private OrderManager theOrder;
    private AudioManager theAudio;
    private OKOrCancel theOOC;

    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;

    //인벤토리 슬롯들
    private InventorySlot[] slots;

    //플레이어가 소지한 아이템 리스트
    private List<Item> inventoryItemList;
    //선택한 탭에 따른 아이템 리스트
    private List<Item> inventoryTabList;

    //아이템 부연설명
    public Text Description_Text;
    //탭 부연설명
    public string[] tabDescription;

    //Slot 부모 객체(부모 객체를 이용하여 InventorySlot 배열에 자식 객체를 넣음)
    public Transform tf;

    //인벤토리 활성화 비활성화
    public GameObject go;
    //탭의 패널들
    public GameObject[] selectedTabImages;
    //선택지를 필요에 따라 활성화, 비활성화
    public GameObject go_OOC;
    public GameObject prefab_Floating_Text;

    //선택된 아이템들을 변수로 확인
    private int selectedItem;
    //선택된 탭
    private int selectedTab;

    //필요한 방향키 기능들만 사용하기 위해
    //인벤토리 활성화 시 true
    private bool activated;
    //탭 활성화 시 true
    private bool tabActivated;
    //아이템 활성화 시 true
    private bool itemActivated;
    //키입력 제한(소비할 때 질의에서 키입력 방지)
    private bool stopKeyInput;
    //중복 실행 제한
    private bool preventExec;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);



    // Start is called before the first frame update
    void Start()
    {
        //instace에 자기 자신을 넣어줌
        instance = this;
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theOOC = FindObjectOfType<OKOrCancel>();

        //리스로 만듦
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        //InventorySlot이 가지고 있는 타입을 slot에 넣음
        //tf(Grid Slot)의 자식 객체들이 들어감
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }

    public void GetAnItem(int _itemID, int _count = 1)
    {
        //데이터베이스 아이템 검색
        for (int i = 0; i < theDatabase.itemList.Count; i++)
        {
            //DB의 아이템ID와 파라미터의 ID가 일치하면(데이터베이스에 아이템 발견)
            if (_itemID == theDatabase.itemList[i].itemID)
            {  
                //Instantiate: prefab을 생성시킴
                //생성시킨 prefab을 clone에 넣음
                //var: 정확한 형식을 모를 때 사용하는 타입
                //prefab_Floating_Text를 생성해서 PlayerManager(Player)의 위치에 0도의 각도로 생성)
                var clone = Instantiate(prefab_Floating_Text, PlayerManager.instance.transform.position, Quaternion.Euler(Vector3.zero));
                //clone의 FloatingText.text.text에 텍스트 대입
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " "  + _count + "개 획득 +";
                //clone의 부모 객체 설정(Inventory를 부모 객체로)
                clone.transform.SetParent(this.transform);

                //인벤토리의 크기만큼 검색(소지품에 같은 아이템이 있는지 확인)
                for (int j = 0; j < inventoryItemList.Count; j++)
                {
                    //인벤토리에 있는 아이템의 ID와 파라미터의 ID가 일치하면(소지품에 같은 아이디가 있다면)
                    if (inventoryItemList[j].itemID == _itemID)
                    {
                        if (inventoryItemList[j].itemType == Item.ItemType.Use)
                        {
                            //갯수만 더해줌
                            inventoryItemList[j].itemCount += _count;
                            Debug.Log("갯수만 더해줌");
                        }
                        else
                        {
                            inventoryItemList.Add(theDatabase.itemList[i]);
                            Debug.Log("리스트 추가");
                        }
                        return;
                    }
                }
                //인벤토리 리스트(소지품)에 아이템 추가
                inventoryItemList.Add(theDatabase.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        //데이터베이스에 itemID 없음
        Debug.LogError("데이터베이트에 해당 ID값을 가진 아이템이 존재하지 않습니다.");
    }

    //인벤토리 슬롯 초기화
    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //전부 값이 없음으로 바꿈(디폴트 슬롯 없앰)
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    //탭 활성화
    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }

    //선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값을 0으로 조정
    public void SelectedTab()
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            //전부 투명하게 만듦
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        //selectedTab(선택된 탭)의 설명이 출력됨
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    //선택된 탭 반짝임 효과
    IEnumerator SelectedTabEffectCoroutine()
    {
        //선택할 수 있는 탭이 활성화돼있다면 계속 반짝이게
        while (tabActivated)
        {
            Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                //선택된 탭만 반짝이게
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                //선택된 탭만 반짝이게
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    //아이템 활성화(inventoryTabList에 조건에 맞는 아이템들만 넣어주고, 인벤토리 슬롯에 출력)
    public void ShowItem()
    {
        //탭 이동할 때 기존의 리스트 초기화(아이템 겹침 방지)
        inventoryTabList.Clear();
        //기존의 아이템 겹쳐서 출력 방지
        RemoveSlot();

        //처음 탭을 고르면 첫 번째 아이템이 선택되게
        selectedItem = 0;

        //탭에 따른 아이템 분류 후 인벤토리 탭 리스트에 추가
        switch (selectedTab)
        {
            case 0:
                //플레이어가 소지한 모든 아이템를 검색
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    //소모품일 경우
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        //탭 리스트에 추가함
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                //플레이어가 소지한 모든 아이템를 검색
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    //장비일 경우
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                        //탭 리스트에 추가함
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                //플레이어가 소지한 모든 아이템를 검색
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    //퀘스트템일 경우
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        //탭 리스트에 추가함
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                //플레이어가 소지한 모든 아이템를 검색
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    //기타 아이템일 경우
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        //탭 리스트에 추가함
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }

        //인벤토리 탭 리스트의 내용을 인벤토리 슬롯에 추가
        for (int i = 0; i < inventoryTabList.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            //텍스트와 아이콘 출력
            slots[i].Additem(inventoryTabList[i]);
        }

        SelectedItem();
    }

    //선택된 아이템을 제외하고, 다른 모든 탭의 컬러 알파값을 0으로 조정
    public void SelectedItem()
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            ////다른 걸 선택했을 때 지나갔던 슬롯의 색 기본값으로 초기화(Panel 색 변경)
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;
            //selectedItem(선택된 아이템)의 itemDescription을 설명에 넣음
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
            Description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
    }

    //선택된 아이템 반짝임 효과
    IEnumerator SelectedItemEffectCoroutine()
    {
        //선택할 수 있는 탭이 활성화돼있다면 계속 반짝이게
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                //선택된 탭만 반짝이게
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                //선택된 탭만 반짝이게
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                //true면 false로, false면 true로 바꿈
                activated = !activated;

                if (activated)
                {
                    theAudio.Play(open_sound);
                    theOrder.NotMove();
                    //인벤토리 창 활성화
                    go.SetActive(true);
                    //탭 초기화
                    selectedTab = 0;
                    //탭부터 고를 수 있도록
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }

            if (activated)
            {
                //탭 활성화 시 키입력 처리
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        //Length: 배열은 0,1,2,3이지만 길이는 4이므로 1을 빼줌
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        //아이템창 활성화
                        itemActivated = true;
                        //탭 비활성화
                        tabActivated = false;
                        //중복 실행 제한
                        preventExec = true;
                        ShowItem();
                    }
                }

                //아이템 활성화 시 키입력 처리
                else if (itemActivated)
                {
                    //아이템이 있어야 방향키 작동
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 2)
                                selectedItem += 2;
                            else
                                selectedItem %= 2;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem > 1)
                                selectedItem -= 2;
                            else
                                selectedItem = inventoryTabList.Count - 1 - selectedItem;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                                selectedItem++;
                            else
                                selectedItem = 0;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem > 0)
                                selectedItem--;
                            else
                                selectedItem = inventoryTabList.Count - 1;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            //0: 소모품
                            if (selectedTab == 0)
                            {
                                theAudio.Play(enter_sound);
                                //입력 막음
                                stopKeyInput = true;
                                //물약을 마실 거냐? 같은 선택지 호출
                                StartCoroutine(OOCCoroutine());
                            }
                            //1: 장비
                            else if (selectedTab == 1)
                            {
                                //장비 장착
                            }
                            //비프음 출력
                            else
                            {
                                theAudio.Play(beep_sound);
                            }
                        }
                    }
                    //아이템 리스트에서 탭으로 나오게 됨
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }

                //중복 실행 방지
                if (Input.GetKeyUp(KeyCode.Z))
                    preventExec = false;
            }
        }
    }

    //OK or Cancel 코루틴
    IEnumerator OOCCoroutine()
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice("사용", "취소");
        //theOOC.activated가 false가 될 때까지 대기
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                //선택된 탭의 선택된 아이템과 같을 경우
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    //아이템 사용 효과
                    theDatabase.Useitem(inventoryItemList[i].itemID);

                    //두 개 이상: 개수 감소, 하나: 리스트 제거
                    if (inventoryItemList[i].itemCount > 1)
                        inventoryItemList[i].itemCount--;
                    else
                        inventoryItemList.RemoveAt(i);

                    //아이템 먹는 소리 출력
                    // theAudio.Play()
                    ShowItem();
                    break;
                }
            }
        }

        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}
