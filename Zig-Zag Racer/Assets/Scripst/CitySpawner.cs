using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySpawner : MonoBehaviour
{
    public static CitySpawner instance;

    public GameObject[] city;

    Camera camera;

    List<GameObject> citys = new List<GameObject>();

    public bool delete = true;

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

    public void SpawnCity(Vector3 pos)
    {
        GameObject newCity = Instantiate(city[0]) as GameObject;

        newCity.transform.SetParent(this.transform, false);

        //right-left
        float x = pos.x + Random.Range(0, 75);
        //forward-backward
        float z = pos.z + 10;

        newCity.transform.position = new Vector3(x, -15, z);

        newCity.SetActive(true);

        citys.Add(newCity);

        StartCoroutine(nameof(DeletesLastCity));        
    }

    IEnumerator DeletesLastCity()
    {
        yield return new WaitForSeconds(10);

        while (delete)
        {
            Debug.Log("asd");
            for (int i = 0; i < city.Length; i++)
            {
                float z = CarController.instance.transform.position.z;
                float x = CarController.instance.transform.position.x;
                var city = citys[i];

                if (city.transform.position.z < z - 10)
                {
                    Destroy(city);
                    citys.Remove(city);
                }
            }
            yield return new WaitForSeconds(2);
        }
    }
}
