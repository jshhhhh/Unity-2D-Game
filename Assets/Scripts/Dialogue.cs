using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //두 줄 입력할 수 있게 하는 명령어
    [TextArea(1, 2)]
    //대사(문장)를 배열로 만듦
    public string[] sentences;
    public Sprite[] sprites;
    //대화창 이름(누가 대화하는지 구분하기 위해)
    public Sprite[] dialogueWindows;

}