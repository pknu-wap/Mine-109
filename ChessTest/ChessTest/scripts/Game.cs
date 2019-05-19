using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ChessTest {
    public class Game : MonoBehaviour {
        public int nowTurn;
        public GameObject Tooltip;
        public List<Controller> players;
        public void Start() {
            nowTurn = 0;
            players[nowTurn].NowPlaying = true;
        }
        bool goNext = false;
        public void NextTurn() {
            goNext = true;
        }
        public void LateUpdate() {//차례 전환
            if (goNext) {
                players[nowTurn].NowPlaying = false;
                nowTurn = (nowTurn + 1) % players.Count; //0과 1 반복
                Debug.Log("차례 : " + nowTurn); //차례:0 과 차례:1 반복
                players[nowTurn].NowPlaying = true;
                goNext = false;//if문 빠져나감 다음 턴 되면 goNext=true 되서 다시 LateUpdate()반복
            }
        }
    }
}
