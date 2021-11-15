using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public GameObject go;

    private bool flag;

    private void OnTriggerEnter2D(Collider2D collition) {
        if(!flag)
        {
            flag = true;
            go.SetActive(true);
        }
    }
}
