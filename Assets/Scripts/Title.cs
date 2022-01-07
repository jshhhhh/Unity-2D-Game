using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private FadeManager theFade;
    private AudioManager theAudio;
    public string click_sound;
    private PlayerManager thePlayer;
    private GameManager theGM;

    private SaveNLoad theSaveNLoad;

    // Start is called before the first frame update
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theGM = FindObjectOfType<GameManager>();
        theSaveNLoad = FindObjectOfType<SaveNLoad>();

        thePlayer.notMove = true;
    }

    public void StartGame()
    {
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        theFade.FadeOut();
        theAudio.Play(click_sound);
        yield return new WaitForSeconds(2f);
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        thePlayer.currentMapName = "start";
        thePlayer.currentSceneName = "start";

        theGM.LoadStart();

        SceneManager.LoadScene("start");
    }

    public void ExitGame()
    {
        theAudio.Play(click_sound);
        Application.Quit();
    }

    public void LoadGame()
    {
        theSaveNLoad.CallLoad();
    }
}
