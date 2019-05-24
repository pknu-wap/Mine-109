using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChessTest
{
    public class ChessBoard : MonoBehaviour
    {
        public List<ChessPiece> pieces;
        public ChessPiece prefab;
        public Material white, black;
        public int size;
        //int getIndex()함수도 없어도 게임은 돌아가네요 알아보겠습니다.
        int getIndex(int x, int z)
        {
            return size * x + z; //size는 unity에서 8이라고 지정해줄거임
        }
        public void Start()
        {
            pieces = new List<ChessPiece>();
            ChessPiece t;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    bool isWhite = (i + j) % 2 == 0;//0과1반복
                    t = Instantiate(prefab, transform); //게임중에 실시간 오브젝트 생성 : instatiate(생성할 오브젝트, 생성할 위치)
                    t.X = i;
                    t.Z = j;
                    t.isWhite = isWhite;
                    t.GetComponent<MeshRenderer>().material.color = isWhite ? Color.white : Color.black;//하얀색검정색번갈아가며 색칠
                    t.transform.position = new Vector3(t.X, -.5f, t.Z);//체스판 64개 하나하나 배치
                    pieces.Add(t);
                }

            }
        }
    }
}