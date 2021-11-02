using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;

    protected bool npcCanMove = true;

    protected Vector3 vector;

    public int walkCount;
    protected int currentWalkCount;
    public Animator animator;
    public BoxCollider2D boxCollider;
    //통과가 불가능한 레이어 설정
    public LayerMask layerMask;

    protected void Move(string _dir, int _frequency)
    {
        StartCoroutine(MoveCoroutine(_dir, _frequency));
    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        npcCanMove = false;
        //switch문을 돌고 나면 벡터 초기화
        vector.Set(0, 0, vector.z);
        switch (_dir)
        {
            case "UP":
                vector.y = 1f;
                break;
            case "DOWN":
                vector.y = -1f;
                break;
            case "RIGHT":
                vector.x = 1f;
                break;
            case "LEFT":
                vector.x = -1f;
                break;
        }

        animator.SetFloat("DirX", vector.x);
        animator.SetFloat("DirY", vector.y);
        animator.SetBool("Walking", true);

        while (currentWalkCount < walkCount)
        {
            //Translate: 현재 있는 값에서 저 수치만큼 더해줌
            //vector.x * speed: vecter.x의 값은 좌 방향키값(-1) 또는 우 방향키값(1) 이 리턴되므로 -1 * speed 또는 1 * speed가 됨
            transform.Translate(vector.x * speed, vector.y * speed, 0);
            //transform.position = vector로도 움직일 수 있음

            currentWalkCount++;
            //0.01초 동안 코루틴 대기
            yield return new WaitForSeconds(0.01f);
        }

        currentWalkCount = 0;
        if(_frequency != 5)
            //애니메이션 다시 멈춤
            animator.SetBool("Walking", false);
        npcCanMove = true;
    }

    protected bool CheckCollision()
    {
        //A지점에서 레이저를 쏴서 B지점에 무사히 도달함: hit = Null;
            //레이저가 방해물에 막히면: hit = 방해물;
            RaycastHit2D hit;

            //A지점, 캐릭터의 현재 위치값
            Vector2 start = transform.position;
            //B지점, 캐릭터가 이동하고자 하는 위치값이 저장됨
            //현재 위치값 + 앞으로 이동하고자 하는 위치값이 저장됨
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

            //캐릭터 자체의 boxCollider에 충돌하는 일을 막기 위해 boxCollider를 끄고 레이저를 쏜 후에 다시 켜줌
            boxCollider.enabled = false;
            //레이저가 end지점까지 도달하는지
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            //LayerMask에 해당하는 벽이 있다면 true 반환, 없으면 false 반환
            if (hit.transform != null)
                return true;
            return false;
    }
}
