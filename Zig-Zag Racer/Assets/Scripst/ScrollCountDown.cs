using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollCountDown : MonoBehaviour
{

    public GameObject panel1;
    public GameObject panel2;

    public TextMeshProUGUI countDownText;

    int countDown = 3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            if (countDown == 0)
            {
                StopCoroutine(CountDown());
                panel2.SetActive(false);
                break;
            }

            countDownText.SetText(countDown + "");

            countDown--;

            yield return new WaitForSeconds(1);
        }
        
    }
}
