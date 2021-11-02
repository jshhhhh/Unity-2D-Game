using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    //배경음악들
    public AudioClip[] clips;

    private AudioSource source;

    //코루틴 안에 new 연산자가 자주 호출되지 않도록 바깥에 따로 선언해줌
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

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

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    //재생(몇 번 트랙을 재생할 건지)
    public void Play(int _playMusicTrack)
    {
        //페이드아웃 후 다시 볼륨을 올려줌
        source.volume = 1f;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void SetVolume(float _volume)
    {
        source.volume = _volume;
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Unpause()
    {
        source.UnPause();
    }

    //정지
    public void Stop()
    {
        source.Stop();
    }

    //서서히 줄어드는 소리
    public void FadeOutMusic()
    {  
        //FadeOut과 FadeIn이 동시에 실행되는 걸 막기 위핸 명령어
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    //코루틴
    IEnumerator FadeOutMusicCoroutine()
    {
        //i가 볼륨(볼륨은 float)
        for (float i = 1.0f; i >= 0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    //서서히 커지는 소리
    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    //코루틴
    IEnumerator FadeInMusicCoroutine()
    {
        //i가 볼륨(볼륨은 float)
        for (float i = 0f; i <= 1f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
