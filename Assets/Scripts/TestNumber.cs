using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNumber : MonoBehaviour
{
    [SerializeField]
    private OrderManager theOrder;
    private NumberSystem theNumber;
    //private DialogueManager theDM;

    public bool flag;
    public int correctNumber;
    //public string[] texts;

    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        //theDM = FindObjectOfType<DialogueManager>();
        theNumber = FindObjectOfType<NumberSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!flag)
        {
            StartCoroutine(ACoroutine());
        }
    }

    IEnumerator ACoroutine()
    {
        flag = true;
        theOrder.NotMove();
        theNumber.ShowNumber(correctNumber);
        //theDM.ShowText(texts);

        //ChoiceManager의 choiceIng이 false가 될 때까지 대기
        yield return new WaitUntil(() => !theNumber.activated);
        //yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
    }
}
