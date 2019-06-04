using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random : MonoBehaviour
{
    public GameObject boxprefab;
    public GameObject bombprefab;
    public static random Instance;
    public static random Instancebomb;
    public string[,] BoxState = new string[8, 8];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                BoxState[i, j] = "o";
            }
        }
        CreateBox();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void CreateBox()
    {
        int i;
        for (i = 0; i < 4;)
        {
            if (SetBox((int)Random.Range(0f, 7f), (int)Random.Range(0f, 7f)))
            {
                i++;
            }
            if (i == 3)
                CreateBomb();
        }

    }
    bool SetBox(int x, int y)
    {
        if (x == 4 && y == 4)
            return false;
        if (x == 2 && y == 2)
            return false;

        if (BoxState[x, y] == "o")
        {
            Instantiate(boxprefab, new Vector3(x, 0.3f, y), Quaternion.Euler(new Vector3(-90, 0, 0)));            

            BoxState[x, y] = "x";
            return true;
        }
        else
            return false;
    }
    void CreateBomb()
    {
        for (int i = 0; i < 10;)
        {   
            if (SetBomb((int)Random.Range(0f, 7f), (int)Random.Range(0f, 7f)))
            {
                i++;
            }
        }
    }
    bool SetBomb(int x, int y)
    {
        if (x == 4 && y == 4)
            return false;
        if (x == 2 && y == 2)
            return false;

        if (BoxState[x, y] == "o")
        {
            Instantiate(bombprefab, new Vector3(x, 0, y), Quaternion.identity);

            BoxState[x, y] = "x";
            return true;
        }
        else
            return false;
    }

}
