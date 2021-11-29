using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBGM : MonoBehaviour
{
    BGMManager BGM;

    public int playMusicTrack;

    // Start is called before the first frame update
    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        BGM.Play(playMusicTrack);
        //충돌하면 꺼줌
        this.gameObject.SetActive(false);
    }
}
