using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgrbomb : MonoBehaviour
{

    public static SpawnMgrbomb Instancebomb;

    public Transform SpawnPointbomb;

    public GameObject bombPrefab;

    //public float createTime = 2.0f;
    public int maxBomb = 4;

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
        Instancebomb = this;
        if (GetChild(SpawnPointbomb).Length > 0)
        {
            StartCoroutine(this.CreateBomb());
        }
    }

    IEnumerator CreateBomb()
    {
        //4개 생성 루프 
        List<Transform> transforms = new List<Transform>();
        transforms.AddRange(GetChild(SpawnPointbomb));
        while (transforms.Count > 0)
        {
            //yield return new WaitForSeconds(createTime);
            int idxbomb = (int)(Random.value * (transforms.Count - 1));
            Instantiate(bombPrefab, transforms[idxbomb].position, transforms[idxbomb].rotation);
            transforms.RemoveAt(idxbomb);
            
            //4개 생성 완료+리스트 개수 0
        }
        while (!isGameOver)
        {
            yield return null;
        }
    }

    public IEnumerator ReviveBomb(GameObject bomb, float time)
    {
        bomb.SetActive(false);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        bomb.SetActive(true);
    }


}

