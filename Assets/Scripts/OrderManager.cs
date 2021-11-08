using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    //이벤트 도중 키입력 처리 방지
    private PlayerManager thePlayer;
    //배열은 한 번 고정되면 크기 변경이 안 되기 때문에 사용하지 않음
    //각 마을마다 NPC의 수가 다양하기 때문에 List 선언
    private List<MovingObject> characters;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    //ToList 함수를 돌고 나온 List<MovingObject> 타입을 반환시켜 characters에 대입함
    public void PreLoadCharacter()
    {
        characters = ToList();
    }

    public List<MovingObject> ToList()
    {
        List<MovingObject> tempList = new List<MovingObject>();
        //함수를 나오면 배열이 사라지고 다시 생성되기 때문에 사용 가능
        //FindObjectsOfType: MovingObject가 달린 모든 객체를 찾아서 반환시킴
        MovingObject[] temp = FindObjectsOfType<MovingObject>();

        for(int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }

        return tempList;
    }

    //PlayerManager의 notMove 변수 true로
    public void NotMove()
    {
        thePlayer.notMove = true;
    }

    //PlayerManager의 notMove 변수 false로
    public void Move()
    {
        thePlayer.notMove = false;
    }

    //벽을 뚫음(boxCollider off)
    public void SetThrought(string _name)
    {
        //배열의 크기는 Length, List의 크기는 Count
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = false;
            }
        }
    }

    //벽을 통과 못 하게(boxCollider on)
    public void SetUnThrought(string _name)
    {
        //배열의 크기는 Length, List의 크기는 Count
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = true;
            }
        }
    }

    //캐릭터가 사라지게 함
    public void SetTransparent(string _name)
    {
        //배열의 크기는 Length, List의 크기는 Count
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }

    //캐릭터가 다시 나타나게 함
    public void SetUnTransparent(string _name)
    {
        //배열의 크기는 Length, List의 크기는 Count
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }
        

    public void Move(string _name, string _dir)
    {
        //배열의 크기는 Length, List의 크기는 Count
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].Move(_dir);
            }
        }
    }

    public void Turn(string _name, string _dir)
    {
        //배열의 크기는 Length, List의 크기는 Count
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].animator.SetFloat("DirX", 0f);
                characters[i].animator.SetFloat("DirY", 0f);
                switch(_dir)
                {
                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1f);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1f);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
