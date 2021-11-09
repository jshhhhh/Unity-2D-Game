using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            //이 오브젝트를 다른 씬을 불러올 때마다 파괴시키지 말라는 명령어
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public Text text;
    //sprite - audioclip(mp3), spriterenderer - audiosource(mp3 플레이어) 관계
    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererDialogueWindow;

    //Dialogue 클래스를 List에 하나하나 넣음
    //List는 배열과 다르게 추가, 클리어 가능
    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;

    //대화 진행 상황 카운드
    private int count;

    public Animator animSprite;
    public Animator animDialogueWindow;

    //대화 효과음
    public string typeSound;
    public string enterSound;

    private AudioManager theAudio;

    //대화가 이루어지지 않을 때 z키 입력을 막는 변수
    public bool talking = false;
    //z가 너무 빨리 입력되지 않게 하는 변수
    //아무리 빨라도 텍스트가 나오기 전까진 z키 스킵이 안 됨
    private bool keyActivated = false;
    

    // Start is called before the first frame update
    void Start()
    {
        //초기값
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        theAudio = FindObjectOfType<AudioManager>();
    }

    //인수로 받은 Dialogue 클래스를 for문으로 넣음
    public void ShowDialogue(Dialogue dialogue)
    {
        //대화가 시작되면 talking true로
        talking = true;

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            //리스트에 배열이 다 들어감
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }

        //Appear가 true가 되면서 이미지와 대화창이 안으로 들어옴
        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);
        //대화가 진행된다는 의미의 코루틴
        StartCoroutine(StartDialogueCoroutine());
    }

    public void ExitDialog()
    {
        //기본값으로 초기화
        text.text = "";
        count = 0;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        //Appear가 false가 되면서 이미지와 대화창이 밖으로 사라짐
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);

        //대화가 끝나면 talking false로
        talking = false;
    }

    IEnumerator StartDialogueCoroutine()
    {
        //count가 0이면 아래 if문에서 문제 발생
        if (count > 0)
        {
            //count와 count-1(이전것)과 비교
            //대사바가 달라진다면 대사바와 캐릭터의 이미지가 둘 다 교체됨
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                //Change로 투명도 조절
                animSprite.SetBool("Change", true);
                animDialogueWindow.SetBool("Appear", false);

                //교체가 일어나는 대기값
                yield return new WaitForSeconds(0.2f);
                //대화창 이미지 교체 후 넣어줌
                rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];

                animDialogueWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
            //대사바가 똑같을 경우
            else
            {
                //같은 캐릭터의 대사지만 이미지가 교체될 경우 스프라이트만 교체
                if (listSprites[count] != listSprites[count - 1])
                {
                    //Change로 투명도 조절
                    animSprite.SetBool("Change", true);

                    //교체가 일어나는 대기값
                    yield return new WaitForSeconds(0.1f);
                    //listSprites의 이미지를 SpriteRenderer의 sprite에 교체해서 넣어줌
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];

                    animSprite.SetBool("Change", false);
                }
                //대사바도 똑같고 스프라이트도 똑같을 경우 약간의 대기
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        
        else
        {
            //대화창 이미지 교체 후 넣어줌
            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }

        //텍스트 출력이 이뤄질 때 활성화
        keyActivated = true;

        //count번째 문장의 총 길이만큼 i를 반복
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            //한 글자씩 출력
            text.text += listSentences[count][i];
            //효과음 재생 조건
            if(i % 7 == 1)
            {
                theAudio.Play(typeSound);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && keyActivated)
        {
            //Z키를 누르면 다음 문장으로 넘어감
            if (Input.GetKeyDown(KeyCode.Z))
            {
                keyActivated = false;
                count++;
                text.text = "";
                //엔터사운드
                theAudio.Play(enterSound);
                //count가 배열의 크기(리스트의 개수)와 똑같을 경우
                if (count == listSentences.Count)
                {
                    //코루틴 끝냄
                    StopAllCoroutines();
                    //대화창 사라짐
                    ExitDialog();
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
    }
}