using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MovingObject
{
    //공격 유예
    public float attackDelay;

    //이동 대기 시간(Inspector창에 표시)
    public float inter_MoveWaitTime;
    //이동 대기 시간(이 변수로 계산)
    private float current_interMWT;

    public string atkSound;
    
    //플레이어 좌표값
    private Vector2 PlayerPos;

    //랜덤 움직임을 위한 변수
    private int random_int;
    //base.Move()(MovingObject로부터 상속)의 string dir 변수에 대입
    private string direction;

    public GameObject healthBar;


    // Start is called before the first frame update
    void Start()
    {
        //queue를 통해서 움직임
        queue = new Queue<string>();
        current_interMWT = inter_MoveWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        current_interMWT -= Time.deltaTime;

        if(current_interMWT <= 0)
        {
            //작업을 끝마치고 다시 초기화
            current_interMWT = inter_MoveWaitTime;

            if(NearPlayer())
            {
                Flip();
                return;
            }
    
            RandomDirection();

            //충돌 체크
            if(base.CheckCollision())
                return;

            base.Move(direction);
        }
    }

    //슬라임을 Player의 방향에 따라 뒤집고 공격하는 함수
    private void Flip()
    {
        //transform의 Scale
        Vector3 flip = transform.localScale;
        //Player가 슬라임보다 x좌표가 크다(Player가 오른쪽에 있다)
        if(PlayerPos.x > this.transform.position.x)
            flip.x = -1f;
        else
            flip.x = 1f;
        this.transform.localScale = flip;

        //슬라임에 따라 체력바가 같이 뒤집어지므로
        //체력바를 한 번 더 뒤집어서 원상태로 만듦
        healthBar.transform.localScale = flip;

        //트리거 작동
        animator.SetTrigger("Attack");
        StartCoroutine(WaitCoroutine());
    }

    //공격 딜레이 후 데미지
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(atkSound);
        //대기 후에도 NearPlayer()가 true라면 데미지를 입힘(없다면 공격모션만)
        if(NearPlayer())
            PlayerStat.instance.Hit(GetComponent<EnemyStat>().atk);
    }

    private bool NearPlayer()
    {
        PlayerPos = PlayerManager.instance.transform.position;

        //좌우 판정
        //플레이어와 슬라임의 x축 절대값을 뺀 절대값이 48(설정한 걸음 속도 값) 이하(근사값)
        if(Mathf.Abs(Mathf.Abs(PlayerPos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walkCount * 1.01f)
        {
            //y축이 일치할 때(근사값)
            if(Mathf.Abs(Mathf.Abs(PlayerPos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }

        //위아래 판정
        if(Mathf.Abs(Mathf.Abs(PlayerPos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walkCount * 1.01f)
        {
            if(Mathf.Abs(Mathf.Abs(PlayerPos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    private void RandomDirection()
    {
        //초기화
        vector.Set(0, 0, vector.z);
        //0~3까지 랜덤(4는 포함 안 됨)
        random_int = Random.Range(0, 4);
        switch(random_int)
        {
            case 0:
                vector.y = 1f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = 1f;
                direction = "RIGHT";
                break;
            case 3:
                vector.x = -1f;
                direction = "LEFT";
                break;
            
        }
    }
}
