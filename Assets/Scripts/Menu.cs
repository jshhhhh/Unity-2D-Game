using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //싱글톤
    public static Menu instance;

    #region Singleton
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //이 오브젝트를 다른 씬을 불러올 때마다 파괴시키지 말라는 명령어
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    #endregion Singleton

    //메뉴 전체를 활성화/비활성화
    public GameObject go;
    public AudioManager theAudio;
    public OrderManager theOrder;

    public string call_sound;
    public string cancel_sound;

    //활성화/비활성화 여부를 판단
    private bool activated;

    public void Exit()
    {
        //종료 함수
        Application.Quit();
    }

    public void Continue()
    {
        activated = false;
        go.SetActive(false);
        theAudio.Play(cancel_sound);
        theOrder.Move();
    }

    // Update is called once per frame
    void Update()
    {
        //ESC키
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;

            if (activated)
            {
                theOrder.NotMove();
                go.SetActive(true);
                theAudio.Play(call_sound);
            }
            else
            {
                go.SetActive(false);
                theAudio.Play(cancel_sound);
                theOrder.Move();
            }
        }
    }
}
