using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKOrCancel : MonoBehaviour
{
    private AudioManager theAudio;
    public string key_sound;
    public string enter_sound;
    public string cancel_sound;

    //위, 아래 패널과 텍스트
    public GameObject up_Panel;
    public GameObject down_Panel;

    public Text up_Text;
    public Text down_Text;

    //고를 때까지 대기
    public bool activated;
    //필요할 때만 키입력 가능하게
    private bool keyInput;
    private bool result = true;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void Selected()
    {
        theAudio.Play(key_sound);
        //위아래 어떤 방향키를 누르든 결과가 바뀜
        result = !result;

        if (result)
        {
            up_Panel.gameObject.SetActive(false);
            down_Panel.gameObject.SetActive(true);
        }
        else
        {
            up_Panel.gameObject.SetActive(true);
            down_Panel.gameObject.SetActive(false);
        }
    }

    public void ShowTwoChoice(string _upText, string _downText)
    {
        activated = true;
        result = true;
        //파라미터 텍스트로 바꿔줌
        up_Text.text = _upText;
        down_Text.text = _downText;

        up_Panel.gameObject.SetActive(false);
        down_Panel.gameObject.SetActive(true);

        //처음 Z키로 선택지 UI를 띄우지만, 밑의 Z키(결정)까지 실행되기 때문에 일부러 약간의 유예를 주고 keyInput 값을 true로 변경(중복키 실행 방지)
        StartCoroutine(ShowTwoChoiceCoroutine());
    }

    //결과값 반환
    public bool GetResult()
    {
        return result;
    }

    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        keyInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(enter_sound);
                keyInput = false;
                activated = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(cancel_sound);
                keyInput = false;
                activated = false;
                result = false;
            }
        }
    }
}
