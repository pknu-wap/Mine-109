using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Vector3 targetPos;
    public float Range = 1000f;


    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //메인카메라 화면의 마우스 클릭 지점에 광선을 발사
        RaycastHit hit; //광선 타격 지점

        if (Input.GetMouseButtonUp(0)) //마우스 왼쪽 버튼을 눌렀을 때
        {
            if (Physics.Raycast(ray, out hit, Range)) //만약 Floor에 광선이 범위 안에서 맞았다면
            {
                this.transform.position = hit.transform.position + new Vector3(0, 0.5f, 0);
                Debug.Log(this.transform.position);               
            }
        }

            else
            {
                Debug.Log("반드시 Floor 안을 클릭해야 합니다!"); //Floor에 광선이 맞지 않았을 경우
            }
        }
    }


