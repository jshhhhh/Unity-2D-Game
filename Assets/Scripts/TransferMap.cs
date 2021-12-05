using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    //이동할 맵의 이름
    public string transferMapName;

    public Transform target;
    public BoxCollider2D targetBound;

    public Animator anim_1;
    public Animator anim_2;

    public int door_count;

    //캐릭터가 바라보고 있는 방향
    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction;
    //getfloat("dirX")를 저장하기 위한 변수
    private Vector2 vector;

    [Tooltip("문이 있으면 : true, 문이 없으면 : false")]
    //문이 있냐 없냐를 Inspector창에서 확인
    public bool door;

    //Player 스크립트를 thePlayer 변수로 불러옴
    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private OrderManager theOrder;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        //FindObjectOfType: 하이어라키에 있는 모든 객체의 <> 컴포넌트를 검색해서 리턴(다수 객체)
        //GetComponent: 해당 스크립트가 적용된 객체의 <> 컴포넌트를 검색해서 리턴(단일 객체)
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }

    //들어가는 순간 한 번만 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!door)
        {
            //충돌한 객체의 이름이 Player라면
            if (collision.gameObject.name == "Player")
            {
                StartCoroutine(TransferCoroutine());
            }
        }
    }

    //안에 머물러있을 때 계속 실행
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (door)
        {
            //충돌한 객체의 이름이 Player라면
            if (collision.gameObject.name == "Player")
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    //vector값 얻어옴
                    vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
                    switch (direction)
                    {
                        case "UP":
                            if (vector.y == 1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "DOWN":
                            if (vector.y == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "RIGHT":
                            if (vector.x == 1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "LEFT":
                            if (vector.x == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        default:
                            StartCoroutine(TransferCoroutine());
                            break;
                    };
                }
            }
        }
    }

    IEnumerator TransferCoroutine()
    {
        //SetTransparent를 사용하려면 플레이어를 찾아야 됨
        //List에 플레이어 있게 만듦
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theFade.FadeOut();
        if (door)
        {
            anim_1.SetBool("Open", true);
            if (door_count == 2)
                anim_2.SetBool("Open", true);
        }
        //FadeOut이 이뤄지고 1초 대기
        yield return new WaitForSeconds(0.5f);

        //플레이어가 투명하게 만듦
        theOrder.SetTransparent("player");
        if (door)
        {
            anim_1.SetBool("Open", false);
            if (door_count == 2)
                anim_2.SetBool("Open", false);
        }

        yield return new WaitForSeconds(0.5f);
        //플레이어가 투명하지 않게 만듦
        theOrder.SetUnTransparent("player");
        //이동할 맵의 이름을 저장
        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        yield return new WaitForSeconds(1f);
        theOrder.Move();
    }
}
