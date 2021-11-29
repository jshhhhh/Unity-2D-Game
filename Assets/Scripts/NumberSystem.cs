using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{
    private AudioManager theAudio;
    //방향키 사운드
    public string key_sound;
    //결정키 사운드
    public string enter_sound;
    //오답 & 취소키 사운드
    public string cancel_sound;
    //정답 사운드
    public string correct_sound;

    //배열의 크기. 몇 자릿수
    //ex) 1000의 자리 -> 3
    private int count;
    //선택된 자릿수
    private int selectedTextBox;
    //플레이어가 도출해낸 값
    private int result;
    //정답(result와 비교)
    private int correctNumber;

    private string tempNumber;

    //숫자들을 화면 가운데로 정렬하기 위해
    public GameObject superObject;
    //6개의 패널 필요한 자릿수 갯수만큼 활성화
    public GameObject[] panel;
    public Text[] Number_Text;

    public Animator anim;
    //return new waitUntil을 위해(패스워드 작업이 끝날 때까지 계속 대기)
    public bool activated;
    //키처리 활성화, 비활성화
    private bool keyInput;
    //정딥인지 아닌지의 여부
    private bool correctFlag;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void ShowNumber(int _correctNumber)
    {
        correctNumber = _correctNumber;
        activated = true;
        correctFlag = false;

        //넘어온 숫자를 문자열로 만들어줌
        //length 기능을 이용하기 위해
        //ex) "143451" -> 6자리
        string temp = correctNumber.ToString();
        for(int i = 0; i < temp.Length; i++)
        {
            //count에 자릿수가 들어감
            count = i;
            //panel 활성화
            panel[i].SetActive(true);
            //초기값으로 0
            Number_Text[i].text = "0";
        }

        //자릿수만큼 패널들을 오른쪽으로 이동시킴
        superObject.transform.position = new Vector3(superObject.transform.position.x + (30 * count), superObject.transform.position.y, superObject.transform.position.z);

        //선택된 자릿수 0부터 시작하게
        selectedTextBox = 0;
        //결과값 0으로(아직 선택하지 않음)
        result = 0;
        SetColor();
        anim.SetBool("Appear", true);
        keyInput = true;
    }

    public bool GetResult()
    {
        return correctFlag;
    }

    //위아래 방향키에 따라 숫자 변하게 하는 함수
    public void SetNumber(string _arrow)
    {
        //선택된 자릿수의 텍스트를 int 숫자 형식으로 강제 형변환
        int temp = int.Parse(Number_Text[selectedTextBox].text);

        if(_arrow == "DOWN")
        {
            if(temp == 0)
                temp = 9;
            else
                temp--;
        }
        else if(_arrow == "UP")
        {
            if(temp == 9)
                temp = 0;
            else
                temp++;
        }
        //다시 문자열로 변환
        Number_Text[selectedTextBox].text = temp.ToString();
    }

    public void SetColor()
    {
        Color color = Number_Text[0].color;
        color.a = 0.3f;
        //for문으로 전부 반투명하게
        for(int i = 0; i <= count; i++)
        {
            Number_Text[i].color = color;
        }
        //선택된 것만 짙게
        color.a = 1f;
        Number_Text[selectedTextBox].color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if(keyInput)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("DOWN");
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("UP");
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                theAudio.Play(key_sound);
                if(selectedTextBox < count)
                    selectedTextBox++;
                else
                    selectedTextBox = 0;
                SetColor();
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                theAudio.Play(key_sound);
                if(selectedTextBox > 0)
                    selectedTextBox--;
                else
                    selectedTextBox = count;
                SetColor();
            }
            //결정키
            else if(Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            //취소키
            else if(Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(key_sound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }

    IEnumerator OXCoroutine()
    {
        Color color = Number_Text[0].color;
        color.a = 1f;

        //거꾸로 넣어주는 이유: 0부터 넣으면 숫자가 거꾸로 들어감
        for(int i = count; i >= 0; i--)
        {
            Number_Text[i].color = color;
            tempNumber += Number_Text[i].text;
        }

        yield return new WaitForSeconds(1f);

        //강제 형변환
        result = int.Parse(tempNumber);

        //정답 판별
        if(result == correctNumber)
        {
            theAudio.Play(correct_sound);
            correctFlag = true;
        }
        else
        {
            theAudio.Play(cancel_sound);
            correctFlag = false;
        }

        StartCoroutine(ExitCoroutine());
    }
    IEnumerator ExitCoroutine()
    {
        Debug.Log("우리가 낸 답 = " + result + ", 정답 = " + correctNumber);
        result = 0;

        tempNumber = "";
        anim.SetBool("Appear", false);

        yield return new WaitForSeconds(0.1f);

        for(int i = 0; i <= count; i++)
        {
            panel[i].SetActive(false);
        }
        superObject.transform.position = new Vector3(superObject.transform.position.x - (30 * count), superObject.transform.position.y, superObject.transform.position.z);
        activated = false;
    }
}
