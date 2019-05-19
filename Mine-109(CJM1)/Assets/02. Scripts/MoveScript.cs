using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Vector3 targetPos;
    public float Range = 1000f;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //메인카메라 화면의 마우스 클릭 지점에 광선을 발사
        RaycastHit hit; //광선 타격 지점

        if (Input.GetMouseButtonUp(0)) //마우스 왼쪽 버튼을 눌렀을 때
        {
            if (Physics.Raycast(ray, out hit, Range)) //만약 Floor에 광선이 범위 안에서 맞았다면
            {
                float x = hit.transform.position.x + new Vector3(0, 0.5f, 0).x;
                float y = hit.transform.position.y + new Vector3(0, 0.5f, 0).y;
                float z = hit.transform.position.z + new Vector3(0, 0.5f, 0).z;

                this.transform.position = new Vector3(x, Mathf.Clamp(y, 0.5f, 0.5f), z);
                Debug.Log(this.transform.position);

            }
        }
    }

    void OnTriggerEnter(Collider other) //박스 습득 스크립트
    {
        if(other.gameObject.CompareTag("Box"))
        {
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
        }
    }
}


