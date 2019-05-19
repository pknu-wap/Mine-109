using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    public Transform SpawnPoint;

    public GameObject boxPrefab;

    public float createTime = 2.0f;
    public int maxBox = 10;

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
        if (GetChild(SpawnPoint).Length > 0)
        {
            StartCoroutine(this.CreateBox());
        }
    }

    IEnumerator CreateBox()
    {
        while (!isGameOver)
        {
            int boxCount = (int)GameObject.FindGameObjectsWithTag("Box").Length;
            
            if (boxCount < maxBox)
            {              
                {
                    List<Transform> transforms = new List<Transform>();
                    transforms.AddRange(GetChild(SpawnPoint));
                    while(transforms.Count>0)
                    {
                        yield return new WaitForSeconds(createTime);
                        int idx = (int)(Random.value*(transforms.Count-1));
                        Instantiate(boxPrefab, transforms[idx].position, transforms[idx].rotation);
                        transforms.RemoveAt(idx);
                        Debug.Log("idx = " + idx);
                        Debug.Log("list = {" + transforms[idx] + "}");
                    }
                }
            }

            else
            {
                yield return null;
            }
        }
    }
}
