using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MovingObject
{
    //static으로 intance 변수의 값을 공유함
    static public PlayerManager instance;

    //trasferMap 스크립트에 있는 transferMapName 변수의 값을 저장
    public string currentMapName;

    /*
    //오디오클립을 일일이 추가하는 방법
    //사운드파일
    public AudioClip walkSound_1;
    public AudioClip walkSound_2;

    //사운드 플레이어
    private AudioSource audioSource;
    */

    //string(이름)으로 사운드에 접근
    public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    private AudioManager theAudio;

    public OrderManager theOrder;
    private FadeManager theFade;
    private TransferScene theScene;

    public float runSpeed;
    private float applyRunSpeed;

    //달릴 때 두 칸씩 움직이는 상황을 방지하는 변수
    private bool applyRunFlag;

    //코루틴 반복 실행 방지 변수
    private bool canMove = true;

    //캐릭터가 대사할 때 움직일 수 없게 하는 변수
    public bool notMove = false;

    // Start is called before the first frame update
    void Start()
    {
        //if문을 거치고 instance에 값이 주어짐
        if (instance == null)
        {
            queue = new Queue<string>();
            //이 오브젝트를 다른 씬을 불러올 때마다 파괴시키지 말라는 명령어
            DontDestroyOnLoad(this.gameObject);
            boxCollider = GetComponent<BoxCollider2D>();
            //audioSource = GetComponent<AudioSource>();
            //캐릭터 객체의 Animator Component를 통제하기 위해
            animator = GetComponent<Animator>();
            theAudio = FindObjectOfType<AudioManager>();
            instance = this;

            theOrder = FindObjectOfType<OrderManager>();
            theFade = FindObjectOfType<FadeManager>();
            theScene = FindObjectOfType<TransferScene>();
        }
        //다음 또 if문을 거치면 instance 값이 있으므로 객체가 삭제됨
        else
        {
            Destroy(this.gameObject);
        }
    }

    //코루틴
    IEnumerator MoveCoroutine()
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0 && !notMove)
        {
            //LeftShift 누르면 달리기 구현
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //적용
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                //적용 안 됨
                applyRunSpeed = 0;
                applyRunFlag = false;
            }


            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            //좌나 우로 갈 때, 상하의 정보를 없앰
            if (vector.x != 0)
                vector.y = 0;

            //방향키에 따라 1, -1를 vector.x에 리턴받고, DirX에 연결된 애니메이션 실행
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            //충돌 방지
            bool checkCollisionFlag = base.CheckCollision();
            //true가 반환되면 break(움직이지 않겠다)
            //false가 반환되면 아래의 걷기 코드 실행
            if (checkCollisionFlag || notMove)
                break;

            animator.SetBool("Walking", true);

            //1에서 4 사이 랜덤으로
            int temp = Random.Range(1, 4);
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;
                case 2:
                    theAudio.Play(walkSound_2);
                    break;
                case 3:
                    theAudio.Play(walkSound_3);
                    break;
                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }

            //이런 식으로 함수들 사용 가능
            //theAudio.SetVolume(walkSound_2, 0.5f);      

            //움직이고자 하는 방향으로 boxcollider를 미리 옮겨줌
            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

            while (currentWalkCount < walkCount)
            {                
                if (vector.x != 0)
                {
                    //Translate: 현재 있는 값에서 저 수치만큼 더해줌
                    //vector.x * speed: vecter.x의 값은 좌 방향키값(-1) 또는 우 방향키값(1) 이 리턴되므로 -1 * speed 또는 1 * speed가 됨
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                    //transform.position = vector로도 움직일 수 있음
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                //달려도 한 칸씩 움직이게
                if (applyRunFlag)
                    currentWalkCount++;

                currentWalkCount++;

                //boxcollider가 12만큼 이동하면 다시 원위치시켜줌
                if (currentWalkCount == 10)
                    boxCollider.offset = Vector2.zero;

                //0.01초 동안 코루틴 대기
                yield return new WaitForSeconds(0.01f);

                /*
                //오디오클립을 일일이 추가하는 방법
                //currentWalkCount는 20
                if(currentWalkCount % 9 == 2)
                {
                    //1에서 2 사이 랜덤으로
                    int temp = Random.Range(1, 2);
                    switch(temp)
                    {
                        case 1:
                        //컴포넌트에 public으로 받은 사운드를 추가하고 재생시킴
                        audioSource.clip = walkSound_1;
                        audioSource.Play();
                            break;
                        case 2:
                        audioSource.clip = walkSound_2;
                        audioSource.Play();
                            break;
                    }
                }
                */
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !notMove)
        {
            //Horizontal: 우 방향키는 1, 좌 방향키는 -1 리턴. Vertical: 상 방향키는 1, 하 방향키는 -1 리턴
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                //코루틴 실행 명령어
                StartCoroutine(MoveCoroutine());
            }
        }
    }
}