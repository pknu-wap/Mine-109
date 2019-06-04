using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxkey : MonoBehaviour
{
    public GameObject key_goldPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player")) return;
        float rand = Random.value; //0.0~1.0 이내의 랜덤 float값을 가져옵니다. 
        if (rand < 0.3f)
        {
            Instantiate(key_goldPrefab, transform.position, transform.rotation);
            Debug.Log("gold key generated");
        }
        else
        {
            Debug.Log("gold key not generated");
        }
    }
}
