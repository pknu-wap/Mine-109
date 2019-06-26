using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChessTest
{
    public class Controller : MonoBehaviour
    {
        bool nowPlaying;
        public ChessBoard board;
        public Game game;
        public Color color; //color 관련된건 있으나 없으나 게임엔 지장이 없네요 이유는 좀더 생각해보겠습니다.
        Vector3 turnPosition;

        public float BoxSpawnTime = 3.0f;
        public float BombSpawnTime = 3.0f;

        private Animation anim;
        public GameObject ExplosionPrefab;

        //플레이어 지정
        public GameObject playerAvatar;
        //플레이어 체력
        public int playerHp = 100;
        //폭발 데미지
        public int explosionDemage = 100;
        //쉴드 작동 여부
        public bool shieldOn = false;
        //쉴드 개수
        public int shieldCount = 1;

        //열쇠
        public bool getkey = false;

        public bool NowPlaying
        {//차례전환
            get
            {
                return nowPlaying;
            }
            set
            {
                if (value)
                {
                    turnPosition = transform.position;
                    game.Tooltip.transform.position = turnPosition + new Vector3(0, 0.01f, 0);
                    game.Tooltip.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, .5f);

                }
                nowPlaying = value;
            }
        }

        public void Start()
        {
            GetComponent<MeshRenderer>().material.color = color;
            anim = GetComponent<Animation>();
        }

        public void Update()
        {            
            if (nowPlaying)
            {
                //폭탄스캔
                BombScanner();

                if (Input.GetKeyDown(KeyCode.W))
                {//위로 이동
                    MoveValidate(0, 1);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {//아래로 이동
                    MoveValidate(0, -1);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {//좌로 이동
                    MoveValidate(-1, 0);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {//우로 이동
                    MoveValidate(1, 0);
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {//확정 & 다음턴 넘기기
                    game.NextTurn();
                }
               
                //쉴드발동
                Shield();
                //플레이어 사망
                PlayerDie();
            }
        }
        public void MoveValidate(int dirX, int dirZ)
        {//이동
            float tX = transform.position.x, tZ = transform.position.z; //현재 벡터 저장
            tX += dirX;
            tZ += dirZ;
            tX = Mathf.Clamp(tX, 0, board.size - 1);//x좌표는 0~7
            tZ = Mathf.Clamp(tZ, 0, board.size - 1);//z좌표는 0~7
            if (Mathf.Abs(turnPosition.x - tX) <= 1 && Mathf.Abs(turnPosition.z - tZ) <= 1)
            {//상하좌우 1칸 제한(절대값이용)
                transform.position = new Vector3(tX, transform.position.y, tZ);//이동
            }
        }
      
       
        public void OnTriggerStay(Collider other) //박스 & 지뢰 습득 스크립트
        {
            if (other.gameObject.CompareTag("Box")&&Input.GetKey(KeyCode.Space))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.gameObject.CompareTag("Bomb") && Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(ReviveBomb(other.gameObject, BombSpawnTime));
                Explosion((int)transform.position.x,(int)transform.position.z);
                Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            }
            //폭탄 데미지
            else if(other.gameObject.CompareTag("Explosion"))
            {
                playerHp -= explosionDemage;
                if(shieldOn == true)
                {
                    shieldOn = false;
                    explosionDemage = 100;
                }
                other.gameObject.SetActive(false);                 
            }
            //열쇠
            else if (other.gameObject.CompareTag("Key") && Input.GetKey(KeyCode.Space))
            {
                other.gameObject.SetActive(false);
                Debug.Log("열쇠획득");
                getkey = true;
            }
        }
        public void Explosion(int positionx, int positionz) {
            for(int i = 0; i < 8; i++)
            {   
                 if(i==positionz)
                    continue;
                Vector3 Positionheight = new Vector3(positionx, 0, i);
                Instantiate(ExplosionPrefab, Positionheight, transform.rotation);
            }

            for(int j=0; j<8; j++)
            {
                if (j == positionx)
                    continue;
                Vector3 PositionWidth = new Vector3(j, 0, positionz);
                Instantiate(ExplosionPrefab, PositionWidth, transform.rotation);
            }
        }

        //쉴드 기능 스크립트
        public void Shield()
        {            
            if (Input.GetKeyDown(KeyCode.E))
            {                    
                if (shieldCount > 0)
                {
                    Debug.Log("쉴드 발동!!");
                    shieldCount -= 1;
                    shieldOn = true;
                    if (shieldOn == true)
                    {
                        explosionDemage = 0;
                    }
                }
                else
                {
                    Debug.Log(game.players[game.nowTurn] + "는 쉴드를 사용할 수 없습니다!");
                }
            }
        }

        //폭탄 스캐너
        public void BombScanner()
        {           
            Vector3 PlayerPosition = playerAvatar.transform.position;
            GameObject[] Bomb = GameObject.FindGameObjectsWithTag("Bomb");
            List<GameObject> GoodBomb = new List<GameObject>();

            //상하좌우
            //상
            List<GameObject> UpBomb = new List<GameObject>();
            //좌
            List<GameObject> LeftBomb = new List<GameObject>();
            //하
            List<GameObject> DownBomb = new List<GameObject>();
            //우
            List<GameObject> RightBomb = new List<GameObject>();

            int BombCount = Bomb.Length;
            for (int i = 0; i < BombCount; i++)
            {
                Vector3 BombPosition = Bomb[i].transform.position;
                float BombDistance = Vector3.Distance(BombPosition, PlayerPosition);

                if(BombDistance <= Mathf.Sqrt(2))
                {
                    GoodBomb.Add(Bomb[i]);
                }                
            }

            for (int j = 0; j < GoodBomb.Count; j++)
            {
                Vector3 GoodBombPosition = GoodBomb[j].transform.position;
                //폭탄 벡터와 플레이어 벡터의 차
                Vector3 DistanceVector = GoodBombPosition - PlayerPosition;

                //상
                Vector3 upVector = (PlayerPosition + new Vector3(1, 0, 1)) - PlayerPosition;
                //좌
                Vector3 LeftVector = (PlayerPosition + new Vector3(-1, 0, 1)) - PlayerPosition;
                //하
                Vector3 DownVector = (PlayerPosition + new Vector3(-1, 0, -1)) - PlayerPosition;
                //우
                Vector3 RightVector = (PlayerPosition + new Vector3(-1, 0, 1)) - PlayerPosition;

                //상 코사인 값
                float Uptheta = Vector3.Dot(DistanceVector, upVector) / (DistanceVector.magnitude * upVector.magnitude);
                //좌 코사인 값
                float Lefttheta = Vector3.Dot(DistanceVector, LeftVector) / (DistanceVector.magnitude * LeftVector.magnitude);
                //하 코사인 값
                float Downtheta = Vector3.Dot(DistanceVector, DownVector) / (DistanceVector.magnitude * DownVector.magnitude);
                //우 코사인 값
                float Righttheta = Vector3.Dot(DistanceVector, RightVector) / (DistanceVector.magnitude * RightVector.magnitude);

                //상 각도
                float UpAngle = Mathf.Acos(Uptheta) * Mathf.Rad2Deg;
                //좌 각도
                float LeftAngle = Mathf.Acos(Lefttheta) * Mathf.Rad2Deg;
                //하 각도
                float DownAngle = Mathf.Acos(Downtheta) * Mathf.Rad2Deg;
                //우 각도
                float RightAngle = Mathf.Acos(Righttheta) * Mathf.Rad2Deg;

                //상 반시계
                float UpCounterClockWise = Vector3.Cross(DistanceVector, upVector).y;
                //좌 반시계
                float LeftCounterClockWise = Vector3.Cross(DistanceVector, LeftVector).y;
                //하 반시계
                float DownCounterClockWise = Vector3.Cross(DistanceVector, DownVector).y;
                //우 반시계
                float RightCounterClockWise = Vector3.Cross(DistanceVector, RightVector).y;

                if (UpCounterClockWise > 0 && UpAngle <= 90 && UpAngle >=0)
                {
                    UpBomb.Add(Bomb[j]);
                }

                else if (LeftCounterClockWise > 0 && LeftAngle <= 90 && LeftAngle >=0)
                {
                    LeftBomb.Add(Bomb[j]);
                }

                else if (DownCounterClockWise > 0 && DownAngle <= 90 && DownAngle >= 0)
                {
                    DownBomb.Add(Bomb[j]);
                }

                else if (RightCounterClockWise > 0 && RightAngle <= 90 && RightAngle >= 0)
                {
                    RightBomb.Add(Bomb[j]);
                }                
            }

            Debug.Log(playerAvatar + "의 주변의 폭탄 갯수는" + "상" + UpBomb.Count + "좌" + LeftBomb.Count + "하" + DownBomb.Count + "우" + RightBomb.Count);
        }

        public void PlayerDie()
        {
            if(playerHp <= 0)
            {
                Destroy(playerAvatar);
                Destroy(game.Tooltip);
                Debug.Log("게임이 종료되었습니다!");
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
            bomb.SetActive(false);
        }
    }
}