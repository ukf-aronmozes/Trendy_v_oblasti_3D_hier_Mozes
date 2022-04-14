using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public static Diamond instance;

    public GameObject diamond;

    private string tagCar = "Player";

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == tagCar)
        {
            DestroyImmediate(gameObject);
        }
    }

    public void SpawnDiamond(Vector3 position)
    {
        if (diamond != null)
        {
            int x = 125;
            int random;

            if ((GameManager.instance.score / 10) != 0)
            {
                x = 25 + (150 / (GameManager.instance.score / 10));
            }
            
            random = Random.Range(0, x);

            if (random < 10)
            {
                position.y += 1.2f;
                Instantiate(diamond, position, diamond.transform.rotation);
            }
        }
    }

    
}
