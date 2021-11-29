using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{
    //이동할 맵의 이름
    public string transferMapName;

    //Player 스크립트를 thePlayer 변수로 불러옴
    private PlayerManager thePlayer;
    private CameraManager theCamera;

    private FadeManager theFade;
    private OrderManager theOrder;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        //FindObjectOfType: 하이어라키에 있는 모든 객체의 <> 컴포넌트를 검색해서 리턴(다수 객체)
        //GetComponent: 해당 스크립트가 적용된 객체의 <> 컴포넌트를 검색해서 리턴(단일 객체)
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }

    IEnumerator TransferCoroutine()
    {
        thePlayer.notMove = true;
        thePlayer.boxCollider.offset = Vector2.zero;
        theFade.FadeOut();
        //FadeOut이 이뤄지고 1초 대기
        yield return new WaitForSeconds(1f);
        //이동할 맵의 이름을 저장
        thePlayer.currentMapName = transferMapName;
        //씬 불러옴
        SceneManager.LoadScene(transferMapName);
        //FadeIn 함수에서 PlayerManager의 notMove를 false로
        theFade.FadeIn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌한 객체의 이름이 Player라면
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }
}