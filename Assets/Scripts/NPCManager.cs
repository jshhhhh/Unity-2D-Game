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
    public int ferquency;
}

public class NPCManager : MovingObject
{
    [SerializeField]
    public NPCMove npc;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveCoroutine());
    }

    // Update is called once per frame
    public void SetMove()
    {

    }

    public void SetNotMove()
    {

    }

    IEnumerator MoveCoroutine()
    {
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {
                switch(npc.ferquency)
                {
                    case 1:
                        yield return new WaitForSeconds(4f);
                        break;
                    case 2:
                        yield return new WaitForSeconds(3f);
                        break;
                    case 3:
                        yield return new WaitForSeconds(2f);
                        break;
                    case 4:
                        yield return new WaitForSeconds(1f);
                        break;
                    case 5:
                        break;
                }

                //npcCanMove가 true가 될 때까지 무한히 대기(case 5가 무한히 반복되는 것을 막기 위해)
                yield return new WaitUntil(() => npcCanMove);

                //실질적인 이동구간
                base.Move(npc.direction[i], npc.ferquency);

                //i가 direction 배열의 크기와 같아지면 -1를 대입하여 반복되게 만듦
                if(i == npc.direction.Length - 1)
                    i = -1;
            }
        }
    }
}