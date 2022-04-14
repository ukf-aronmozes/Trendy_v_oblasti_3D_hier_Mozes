using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public static Camera_Follow instance;

    public Transform target;
    public float smoothValue;

    Quaternion a;

    Vector3 distance;

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
        distance = target.position - transform.position;

        a = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.position.y >= 0)
        {
            Follow();
        }
        else if (target.position.y >= -2)
        {
            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.Euler(31.242f + 100f, 44.622f, 0);
            transform.rotation = Quaternion.Lerp(from , to, (smoothValue / 6) * Time.deltaTime);
        }
    }

    void Follow()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = target.position - distance;

        transform.position =  Vector3.Lerp(currentPosition, targetPosition, smoothValue * Time.deltaTime);
    }

    public void ResetCamera()
    {
        transform.rotation = a;
    } 
}
