using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Bound[] bounds;
    private PlayerManager thePlayer;
    private CameraManager theCamera;

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

        //하이어라키의 Player를 찾아서 theCamera의 target을 바꿈
        theCamera.target = GameObject.Find("Player");

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
    }
}
