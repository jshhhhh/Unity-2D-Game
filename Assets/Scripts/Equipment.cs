using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private OrderManager theOrder;
    private AudioManager theAudio;
    private PlayerStat thePlayerStat;
    private Inventory theInven;
    private OKOrCancel theOOC;
    private Equipment theEquip;

    public string key_sound;
    public string enter_sound;
    public string open_sound;
    public string close_sound;
    public string takeoff_sound;
    public string equip_sound;

    //const: 변수를 상수처럼 만듦(다른 곳에서 값을 바꿀 수 없음)
    //숫자 대신 변수를 대입
    private const int WEAPON = 0, SHILD = 1, AMULET = 2, LEFT_RING = 3, RIGHT_RING = 4, HELMET = 5, ARMOR = 6, LEFT_GLOVE = 7, RIGHT_GLOVE = 8, BELT = 9, LEFT_BOOTS = 10, RIGHT_BOOTS = 11;

    private const int ATK = 0, DEF = 1, HPR = 6, MPR = 7;

    private int added_atk, added_def, added_hpr, added_mpr;


    public GameObject equipWeapon;
    //장비창 키고 끔
    public GameObject go;
    public GameObject go_OOC;

    //스탯
    public Text[] text;
    //장비 슬롯 아이콘
    public Image[] img_slots;
    //선택된 장비 슬롯 UI
    public GameObject go_selected_Slot_UI;

    //장착된 장비 리스트(편의상 List로 명명)
    public Item[] equipItemList;

    //선택된 장비 슬롯
    private int selectedSlot;

    public bool activated = false;
    private bool inputKey = true;


    // Start is called before the first frame update
    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theOOC = FindObjectOfType<OKOrCancel>();
        theEquip = FindObjectOfType<Equipment>();
    }

    //스탯을 텍스트로 표기
    public void ShowTxT()
    {
        if(added_atk == 0)
            text[ATK].text = thePlayerStat.atk.ToString();
        else
            text[ATK].text = thePlayerStat.atk.ToString() + "(+" + added_atk + ")";

        if(added_def == 0)
            text[DEF].text = thePlayerStat.def.ToString();
        else
            text[DEF].text = thePlayerStat.atk.ToString() + "(+" + added_def + ")";

        if(added_hpr == 0)
            text[HPR].text = thePlayerStat.recover_hp.ToString();
        else
            text[HPR].text = thePlayerStat.recover_hp.ToString() + "(+" + added_hpr + ")";

        if(added_mpr == 0)
            text[MPR].text = thePlayerStat.recover_mp.ToString();
        else
            text[MPR].text = thePlayerStat.recover_mp.ToString() + "(+" + added_mpr + ")";
    }

    public void EquipItem(Item _item)
    {
        //temp에 itemID를 넣음(문자열로 형변환)
        string temp = _item.itemID.ToString();
        //앞글자 0번째부터 3번째 글자까지 자름
        temp = temp.Substring(0, 3);
        //앞글자 3자리로 장비를 분류함
        switch (temp)
        {
            //무기
            case "200":
                EquipItemCheck(WEAPON, _item);
                equipWeapon.SetActive(true);
                //스프라이트를 인수로 넘어온 아이템 아이콘으로 교체
                equipWeapon.GetComponent<SpriteRenderer>().sprite = _item.itemIcon;
                break;
            //방패
            case "201":
                EquipItemCheck(SHILD, _item);
                break;
            //아뮬렛
            case "202":
                EquipItemCheck(AMULET, _item);
                break;
            //반지
            case "203":
                EquipItemCheck(LEFT_RING, _item);
                break;
        }
    }

    public void EquipItemCheck(int _count, Item _item)
    {
        //아무것도 장착되어 있지 않다면
        if (equipItemList[_count].itemID == 0)
        {
            //그 아이템을 장착
            equipItemList[_count] = _item;
        }
        //장착되어 있다면
        else
        {
            //자기가 차고 있던 장비를 아이템 창에 넣음
            theInven.EquipToInventory(equipItemList[_count]);
            //장착 아이템의 능력치만큼 감소
            TakeOffEffect(equipItemList[_count]);
            //아이템 창에서 그 아이템을 장착
            equipItemList[_count] = _item;
        }

        //장착한 아이템의 효과가 스탯에 반영
        EquipEffect(_item);
        ShowTxT();
        theAudio.Play(equip_sound);
    }

    public void SelectedSlot()
    {
        //선택된 이미지 슬롯의 위치값으로 UI를 이동
        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;
    }

    public void ClearEquip()
    {
        //알파값을 0f로
        Color color = img_slots[0].color;
        color.a = 0f;

        //반복문으로 알파값 넣어줌
        for (int i = 0; i < img_slots.Length; i++)
        {
            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }
    }

    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            //itemID가 0이 아님 -> 무언가 장착됨
            if (equipItemList[i].itemID != 0)
            {
                //아이콘(strite) 대입
                img_slots[i].sprite = equipItemList[i].itemIcon;
                //알파값 1f 대입
                img_slots[i].color = color;
            }
        }
    }

    //장비 -> 능력치만큼 상승
    private void EquipEffect(Item _item)
    {
        thePlayerStat.atk += _item.atk;
        thePlayerStat.def += _item.def;
        thePlayerStat.recover_hp += _item.recover_hp;
        thePlayerStat.recover_mp += _item.recover_mp;

        added_atk += _item.atk;
        added_def += _item.def;
        added_hpr += _item.recover_hp;
        added_mpr += _item.recover_mp;
    }

    //장비 해제 -> 올랐던 능력치만큼 하락
    private void TakeOffEffect(Item _item)
    {
        thePlayerStat.atk -= _item.atk;
        thePlayerStat.def -= _item.def;
        thePlayerStat.recover_hp -= _item.recover_hp;
        thePlayerStat.recover_mp -= _item.recover_mp;
        //장착 해제했을 때 현재 체력보다 최대 체력이 높아지는 현상 방지
        //currentHP = 최대 체력;

        added_atk -= _item.atk;
        added_def -= _item.def;
        added_hpr -= _item.recover_hp;
        added_mpr -= _item.recover_mp;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputKey)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;

                if (activated)
                {
                    theOrder.NotMove();
                    theAudio.Play(open_sound);
                    go.SetActive(true);
                    //창을 열면 첫 번째 슬롯이 자동으로 선택
                    selectedSlot = 0;
                    SelectedSlot();
                    ClearEquip();
                    ShowEquip();
                    ShowTxT();
                }
                else
                {
                    theOrder.Move();
                    theAudio.Play(close_sound);
                    go.SetActive(false);
                    ClearEquip();
                }
            }


            if (activated)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot <= img_slots.Length)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    //빈 슬롯이 아닐 경우에만 실행
                    if (equipItemList[selectedSlot].itemID != 0)
                    {
                        theAudio.Play(enter_sound);
                        inputKey = false;
                        StartCoroutine(OOCCoroutine("벗기", "취소"));
                        //Debug.Log();
                    }
                    else
                    {
                        theAudio.Play(close_sound);
                    }
                }
            }
        }
    }

    //장비 벗기 OK or Cancel 코루틴
    IEnumerator OOCCoroutine(string _up, string _down)
    {
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        //theOOC.activated가 false가 될 때까지 대기
        //키입력 대기
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {
            //선택된 장착 아이템을 인벤토리 아이템 리스트에 추가
            theInven.EquipToInventory(equipItemList[selectedSlot]);
            //차고 있던 장비의 능력치만큼 감소
            TakeOffEffect(equipItemList[selectedSlot]);
            //무기를 벗을 경우에만 무기 이미지를 꺼줌
            if(selectedSlot == WEAPON)
                equipWeapon.SetActive(false);
            ShowTxT();
            //선택된 장착 슬롯에 빈 껍데기를 넣음(없애버리면 참조 문제가 생기기 때문에 X)
            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip);
            theAudio.Play(takeoff_sound);
            ClearEquip();
            ShowEquip();
        }
        inputKey = true;
        go_OOC.SetActive(false);
    }
}
