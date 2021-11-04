using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    [Tooltip("NPCMove를 체크하면 NPC가 움직임")]
    public bool NPCmove;

    //NPC가 움직일 방향 설정
    public string[] direction;

    [Range(1, 5)] [Tooltip("1 = 천천히, 2 = 조금 천천히, 3 = 보통, 4 = 빠르게, 5 = 연속적으로")]
    //NPC가 움직일 방향으로 얼마나 빠른 속오로 움직일 것인가
    public int frequency;
}

public class NPCManager : MovingObject
{
    [SerializeField]
    public NPCMove npc;

    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<string>();
        StartCoroutine(MoveCoroutine());       
    }

    // Update is called once per frame
    public void SetMove()
    {
        
    }

    public void SetNotMove()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveCoroutine()
    {
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {
                //큐가 0과 1일 경우에만 실행, 큐가 2 이상이 되면 기다리다가 큐가 빠지면 1이 되면서 밑의 Move를 실행(애니메이션이 자연스러워지게)
                yield return new WaitUntil(() => queue.Count < 2);

                //실질적인 이동구간
                base.Move(npc.direction[i], npc.frequency);

                //i가 direction 배열의 크기와 같아지면 -1를 대입하여 반복되게 만듦
                if(i == npc.direction.Length - 1)
                    i = -1;
            }
        }
    }
}