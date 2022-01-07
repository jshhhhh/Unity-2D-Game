using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    public Button button;
    public SaveNLoad theSaveNLoad;
    
    void Start()
    {
        button = this.GetComponent<Button>();
        theSaveNLoad = GameObject.Find("Player").GetComponent<SaveNLoad>();
        button.onClick.AddListener(theSaveNLoad.CallLoad);
    }
}
