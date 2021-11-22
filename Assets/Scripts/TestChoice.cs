using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoice : MonoBehaviour
{
    [SerializeField]
    public Choice choice;
    private OrderManager theOrder;
    private ChoiceManager theChoice;

    public bool flag;

    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
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
        theChoice.ShowChoice(choice);

        //ChoiceManager의 choiceIng이 false가 될 때까지 대기
        yield return new WaitUntil(() => !theChoice.choiceIng);

        theOrder.Move();
        //잘 선택됐는지 로그로 확인
        Debug.Log(theChoice.GetResult());
    }
}
