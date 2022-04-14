using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    public static Scroll instance;

    public GameObject panel;
    public GameObject scrollView;
    public GameObject cellPrefab;
    public GameObject container;
    public GameObject arrow;
    GameObject lastButton;

    List<GameObject> slot = new List<GameObject>();

    public GameObject winEffect;

    public Sprite[] backGround;
    public Color[] colors;

    ScrollRect scrollRect;

    float startPosition = 1, endPosition = 0;
    float t0;

    Vector2 lastButtonY;

    static int minRandom, maxRandom, firstItemDroppRate, secoundDroprate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = (RectTransform)container.transform;

        int unlucky = Random.Range(0, 3);

        for (int i = 0; i < 100; i++)
        {   //40. all meg

            int randomRarity;
            int randomCar = 1;
            int randomSkin = Random.Range(0,5);

            switch (unlucky)
            {
                case 0:
                    if (i != 42)
                    {
                        randomRarity = Random.Range(0, 100);
                    }
                    else
                    {
                        randomRarity = 100;
                    }
                    break;
                case 1:
                    if (i != 40)
                    {
                        randomRarity = Random.Range(0, 100);
                    }
                    else
                    {
                        randomRarity = 100;
                    }
                    break;
                case 2:
                    if (i != 42 && i != 40)
                    {
                        randomRarity = Random.Range(0, 100);
                    }
                    else
                    {
                        randomRarity = 100;
                    }
                    break;
                default:
                    randomRarity = 0;
                    break;
            }

            if (randomRarity < firstItemDroppRate)
            {
                randomCar = Random.Range(minRandom, minRandom + 2);
            }
            else
            {
                randomCar = Random.Range(maxRandom - 2, maxRandom);
            }


            GameObject newCell = Instantiate(cellPrefab) as GameObject;

            newCell.transform.SetParent(container.transform, false);
            newCell.GetComponent<Image>().sprite = backGround[randomCar];
            
            Transform currentItem = newCell.transform.GetChild(0);
            var carWithSkin = CarSkinButton.instance.carImages[randomCar].images[randomSkin];
            currentItem.GetComponent<Image>().sprite = carWithSkin;
            currentItem.GetComponent<Image>().SetNativeSize();
            currentItem.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);

            currentItem = newCell.transform.GetChild(1);
            
            currentItem.GetComponent<TextMeshProUGUI>().SetText(randomCar.ToString() + " " + randomSkin.ToString());

            slot.Add(newCell);

            float width = rt.rect.width;
            float height = rt.rect.height;

            container.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height + 100f);
        }

        scrollRect = scrollView.GetComponent<ScrollRect>();

        StartCoroutine(AutoScroll());
    }

    public void CommonChest()
    {
        minRandom = 0;
        maxRandom = 4;
        firstItemDroppRate = 75;
        secoundDroprate = 25;
    }

    public void LegendaryChest()
    {
        minRandom = 2;
        maxRandom = 6;
        firstItemDroppRate = 95;
        secoundDroprate = 5;
    }

    public IEnumerator AutoScroll()
    {
        yield return new WaitForSeconds(3f);
        
        t0 = 0.0f;
        scrollRect.verticalNormalizedPosition = 1;
        float help = 0f;
        float sin = Mathf.Sin(help);
        bool up = true;
        
        while (t0 < 1.0f)
        {
            if (t0 < 0.95)
            {
                t0 += (1 - t0) * 0.01f;
                help = (1 - t0) * 0.01f;
            }
            else
            {
                t0 += help;
            }

            scrollRect.verticalNormalizedPosition = Mathf.Lerp(startPosition, endPosition, t0);
            yield return new WaitForEndOfFrame();
        }
        Vector2 scrollNormalPos = scrollRect.normalizedPosition;

        int scrollAnimation = (int)Random.Range(25,75);

        while (scrollNormalPos.y < lastButtonY.y)
        {
            if(scrollAnimation > 0)
            {
                scrollNormalPos.y -= help;
                scrollAnimation--;
            }
            else
            {
                scrollNormalPos.y += help;
            }
            
            scrollRect.normalizedPosition = scrollNormalPos;
            yield return new WaitForEndOfFrame();
        }
        Transform currentItem = lastButton.transform.GetChild(1);
        string imageText = currentItem.GetComponent<TextMeshProUGUI>().text;
        string[] newIndex = imageText.Split(' ');
        
        winEffect.GetComponent<ParticleSystem>().startColor = colors[int.Parse(newIndex[0])];

        winEffect.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        GameObject newButton = Instantiate(lastButton) as GameObject;
        
        newButton.transform.SetParent(panel.transform, false);

        newButton.transform.localPosition = Vector3.zero;

        int vertical = 300;
        int horizontal = 600;

        while (vertical <= 1920)
        {
            newButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vertical);
            newButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontal);

            vertical += 10;

            if (horizontal < 1080)
            {
                horizontal += 10;
            }

            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(2);
        Debug.Log(newIndex[1]);
        AvailableCars.instance.UnlockCar(int.Parse(newIndex[0]), int.Parse(newIndex[1]));

        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Slot")
        {
            lastButtonY = scrollRect.normalizedPosition;
            lastButton = other.gameObject;
        }
    }
}
