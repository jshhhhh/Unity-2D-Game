using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;

    private Vector3 vector;

    public float runSpeed;
    private float applyRunSpeed;

    //달릴 때 두 칸씩 움직이는 상황을 방지하는 변수
    private bool applyRunFlag;

    public int walkCount;
    private int currentWalkCount;

    //코루틴 반복 실행 방지 변수
    private bool canMove = true;


    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {   //캐릭터 객체의 Animator Component를 통제하기 위해
        animator = GetComponent<Animator>();
    }

    //코루틴: 
    IEnumerator MoveCoroutine()
    {
        while(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            //LeftShift 누르면 달리기 구현
            if(Input.GetKey(KeyCode.LeftShift))
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
            if(vector.x != 0)
                vector.y = 0;

            //방향키에 따라 1, -1를 vector.x에 리턴받고, DirX에 연결된 애니메이션 실행
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);
            animator.SetBool("Walking", true);


            while(currentWalkCount < walkCount)
            {            
                if(vector.x != 0)
                {
                    //Translate: 현재 있는 값에서 저 수치만큼 더해줌
                    //vector.x * speed: vecter.x의 값은 좌 방향키값(-1) 또는 우 방향키값(1) 이 리턴되므로 -1 * speed 또는 1 * speed가 됨
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                    //transform.position = vector로도 움직일 수 있음
                }
                else if(vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                //달려도 한 칸씩 움직이게
                if(applyRunFlag)
                    currentWalkCount++;

                currentWalkCount++;
                //0.01초 동안 코루틴 대기
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            //Horizontal: 우 방향키는 1, 좌 방향키는 -1 리턴. Vertical: 상 방향키는 1, 하 방향키는 -1 리턴
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                //코루틴 실행 명령어
                StartCoroutine(MoveCoroutine());
            }
        }
    }
}
