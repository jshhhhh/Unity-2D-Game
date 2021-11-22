using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;

    #region Singleton
    private void Awake()
    {
        if (instance != null)
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

    //사운드 재생
    private AudioManager theAudio;
    //Choice 커스텀 클래스의 question을 여기에 대입
    private string question;
    //Choice 커스텀 클래스의 answer 배열을 여기에 대입
    private List<string> answerList;

    //평소: SetActive(false), 필요시: SetActive(true)
    public GameObject go;

    //라이브러리에 using UnityEngine.UI; 추가
    public Text question_Text;
    public Text[] answer_Text;
    //선택된 패널만 투명도를 짙게, 나머지는 옅게 하기 위해
    public GameObject[] answer_Panel;

    public Animator anim;

    public string keySound;
    public string enterSound;

    //선택지가 이루어지기 전에 대화를 대기시킴
    public bool choiceIng;
    //키처리 활성화/비활성화
    private bool keyInput;

    //배열의 크기
    private int count;
    //선택한 선택창의 번호
    private int result;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        //리스트, 텍스트 모두 초기화
        answerList = new List<string>();
        for (int i = 0; i < answer_Text.Length; i++)
        {
            answer_Text[i].text = "";
            //선택지 비활성화
            answer_Panel[i].SetActive(false);
        }
        question_Text.text = "";
    }

    public void ShowChoice(Choice _choice)
    {
        go.SetActive(true);
        choiceIng = true;
        result = 0;
        question = _choice.question;
        for (int i = 0; i < _choice.answers.Length; i++)
        {
            //배열의 크기만큼 answerList에 들어감
            answerList.Add(_choice.answers[i]);
            //배열의 크기만큼 패널 활성화(ex. 3이면 1,2,3 패널 활성화)
            answer_Panel[i].SetActive(true);
            count = i;
        }
        anim.SetBool("Appear", true);
        Selection();
        StartCoroutine(ChoiceCoroutine());
    }

    //선택창이 몇 번인지 꺼내오는 함수
    public int GetResult()
    {
        return result;
    }

    //끄고 초기화하는 함수
    public void ExitChoice()
    {
        StartCoroutine(ExitChoiceCoroutine());
    }

    IEnumerator ExitChoiceCoroutine()
    {
        question_Text.text = "";
        for(int i = 0; i <= count; i++)
        {
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        answerList.Clear();
        anim.SetBool("Appear", false);
        choiceIng = false;
        //선택창이 사라지는 애니메이션을 보기 위해 유예
        yield return new WaitForSeconds(0.2f);
        go.SetActive(true);
    }

    IEnumerator ChoiceCoroutine()
    {
        //설정한 애니메이션이 실행될 때까지 유예를 줌
        yield return new WaitForSeconds(0.2f);

        //질문과 첫 번째 답변은 무조건 실행되여야 함
        StartCoroutine(TypingQuestion());
        StartCoroutine(TypingAnswer_0());
        //count(배열의 크기)만큼 답변 출력
        if (count >= 1)
            StartCoroutine(TypingAnswer_1());
        if (count >= 2)
            StartCoroutine(TypingAnswer_2());
        if (count >= 3)
            StartCoroutine(TypingAnswer_3());

        yield return new WaitForSeconds(0.5f);
        keyInput = true;
    }

    IEnumerator TypingQuestion()
    {
        //question의 길이만큼 
        for (int i = 0; i < question.Length; i++)
        {
            //한 글자씩 순서대로 들어감
            question_Text.text += question[i];
            yield return waitTime;
        }
    }

    //코루틴을 나눠줘야 패널에 출력 오류가 생기지 않음
    IEnumerator TypingAnswer_0()
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < answerList[0].Length; i++)
        {
            //한 글자씩 순서대로 들어감
            answer_Text[0].text += answerList[0][i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_1()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < answerList[1].Length; i++)
        {
            //한 글자씩 순서대로 들어감
            answer_Text[1].text += answerList[1][i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_2()
    {
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < answerList[2].Length; i++)
        {
            //한 글자씩 순서대로 들어감
            answer_Text[2].text += answerList[2][i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_3()
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < answerList[3].Length; i++)
        {
            //한 글자씩 순서대로 들어감
            answer_Text[3].text += answerList[3][i];
            yield return waitTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //효과음
                theAudio.Play(keySound);
                //위 방향키를 눌렀을 때 result(선택창)의 값이 1 미만이면 다시 선택창의 최하단으로
                if (result > 0)
                    result--;
                else
                    result = count;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //효과음
                theAudio.Play(keySound);
                //아래 방향키를 눌렀을 때 result(선택창)의 값이 count 이상이면 다시 0(선택창의 최상단)으로
                if (result < count)
                    result++;
                else
                    result = 0;
                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                //효과음
                theAudio.Play(enterSound);
                keyInput = false;
                ExitChoice();
            }
        }
    }

    //투명도를 조절하여 선택창 표시
    public void Selection()
    {
        //판넬의 Image 컴포넌트 사용
        Color color = answer_Panel[0].GetComponent<Image>().color;
        //투명도 0.75f를 반복문으로 널어줌
        color.a = 0.75f;
        for(int i = 0; i <= count; i++)
        {
            answer_Panel[i].GetComponent<Image>().color = color;
        }
        color.a = 1f;
        //선택된 것(result)만 투명도 진하게
        answer_Panel[result].GetComponent<Image>().color = color;
    }
}
