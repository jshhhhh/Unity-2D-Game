using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    //캐릭터의 방향을 불러오기 위해
    private PlayerManager thePlayer;

    //한 번만 실행되게
    private bool flag;


    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GetKeyDown: 계속 누르고 있어도 적용됨
        //flag가 false이고 Z키가 눌리고 플레이어 방향이 1일 때(위로 바라봄)
        if(!flag && Input.GetKey(KeyCode.Z) && thePlayer.animator.GetFloat("DirY") == 1f)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        //OrderManager의 리스트를 채워주는 함수
        theOrder.PreLoadCharacter();

        theOrder.NotMove();

        theDM.ShowDialogue(dialogue_1);

        //DialogueManager의 talking이 false일 경우에만 아래 코드 실행
        //대화가 끝날 때까지 기다렸다가 끝나면 이동시킴
        yield return new WaitUntil(() => !theDM.talking);

        //Move(): 플레이가 직접 움직임 가능
        //Move(객체, 인수): 인수대로 움직이게 명령(큐)
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "UP");

        //위의 움직임이 끝날 때까지(큐가 다 빠질 때까지) 넘어가지 않음
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);

        theDM.ShowDialogue(dialogue_2);

        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
    }
}
