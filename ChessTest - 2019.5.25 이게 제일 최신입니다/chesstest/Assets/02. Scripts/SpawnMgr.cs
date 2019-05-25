using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{

    public static SpawnMgr Instance;

    public Transform SpawnPoint;

    public GameObject boxPrefab;


    //public float createTime = 2.0f;
    public int maxBox = 4;

    public bool isGameOver = false;    

    //Children만 데려오는 함수
    Transform[] GetChild(Transform parent)
    {
        Transform[] childArray = parent.GetComponentsInChildren<Transform>();
        Transform[] child = new Transform[parent.childCount];

        int index = 0;
        foreach (Transform t in childArray)
        {
            if (t.parent == parent)
                child[index++] = t;
        }
        return child;
    }

    void Start()
    {
        Instance = this;
        if (GetChild(SpawnPoint).Length > 0)
        {
            StartCoroutine(this.CreateBox());
        }
    }

    IEnumerator CreateBox()
    {
        //4개 생성 루프 
        List<Transform> transforms = new List<Transform>();
        transforms.AddRange(GetChild(SpawnPoint));
        while (transforms.Count > 0)
        {
            //yield return new WaitForSeconds(createTime);
            int idx = (int)(Random.value * (transforms.Count - 1));
            Instantiate(boxPrefab, transforms[idx].position, transforms[idx].rotation);
            transforms.RemoveAt(idx);           
            
            //4개 생성 완료+리스트 개수 0
        }
        while (!isGameOver)
        {
            yield return null;
        }
    }

    public IEnumerator ReviveBox(GameObject box,float time)
    {
        box.SetActive(false);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        box.SetActive(true);
    }

    
}
