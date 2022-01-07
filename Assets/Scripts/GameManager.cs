using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Bound[] bounds;
    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private Menu theMenu;
    private DialogueManager theDM;
    private Camera cam;

    public GameObject hpbar;
    public GameObject mpbar;

    public void LoadStart()
    {
        StartCoroutine(LoadWaiteCoroutine());
    }

    IEnumerator LoadWaiteCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerManager>();
        //로드할 때마다 씬에 있는 바운드로 바뀜(유예 시간 뒤에)
        bounds = FindObjectsOfType<Bound>();
        theCamera = FindObjectOfType<CameraManager>();
        theFade = FindObjectOfType<FadeManager>();
        theMenu = FindObjectOfType<Menu>();
        theDM = FindObjectOfType<DialogueManager>();
        cam = FindObjectOfType<Camera>();

        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;

        //하이어라키의 Player를 찾아서 theCamera의 target을 바꿈
        theCamera.target = GameObject.Find("Player");
        //캔버스의 월드카메라 속성에 카메라를 넣어줌
        //씬 이동이 이뤄져도 카메라를 찾을 수 있게
        theMenu.GetComponent<Canvas>().worldCamera = cam;
        //DialogueManager에 있는 캔버스를 찾아서 속성 바꿈
        theDM.GetComponent<Canvas>().worldCamera = cam;

        for(int i = 0; i < bounds.Length; i++)
        {
            //캐릭터가 있던 맵의 BoxCollider를 카메라 바운드로 지정하기 위해서
            //CurrentMapName과 BoundName을 조건 비교한 뒤
            //카메라 바운드로 설정해줌
            if(bounds[i].boundName == thePlayer.currentMapName)
            {
                bounds[i].SetBound();
                break;
            }
        }

        hpbar.SetActive(true);
        mpbar.SetActive(true);

        theFade.FadeIn();        
        StartCoroutine(thePlayer.canStartPointMoveCoroutine());
    }
}
