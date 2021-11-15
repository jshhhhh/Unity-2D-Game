using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager instance;

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

    private AudioManager theAudio;
    public ParticleSystem rain;
    public string rain_sound;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    //파티클 재생
    public void Rain()
    {
        theAudio.Play(rain_sound);
        rain.Play();
    }

    //파티클 멈춤
    public void RainStop()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }

    public void RainDrop()
    {
        //10방울만 내리게
        rain.Emit(10);
    }
}
