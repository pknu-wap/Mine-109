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

        //지뢰 개수 표시
        public GameObject BTDUp;
        public GameObject BTDLeft;
        public GameObject BTDDown;
        public GameObject BTDRight;
        /*public GameObject WTDUp;
        public GameObject WTDLeft;
        public GameObject WTDDown;
        public GameObject WTDRight;*/

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

                //폭탄스캔
                ScanBomb();               
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
        //폭탄스캔
        public void ScanBomb()
        {
            GameObject[] Bombs = GameObject.FindGameObjectsWithTag("Bomb");
            int up = 0, down = 0, left = 0, right = 0;
            Vector3 nowPosition = playerAvatar.transform.position;
                     

            foreach (GameObject b in Bombs)
            {
                if ((b.transform.position - nowPosition).magnitude <= Mathf.Sqrt(2))
                {
                    float xdiff = b.transform.position.x - nowPosition.x;
                    float zdiff = b.transform.position.z - nowPosition.z;
                    if (xdiff >= 1) right++;
                    if (xdiff <= -1) left++;
                    if (zdiff >= 1) up++;
                    if (zdiff <= -1) down++;
                }
            }

            BTDUp.GetComponent<TextMesh>().text = up.ToString();
            BTDLeft.GetComponent<TextMesh>().text = left.ToString();
            BTDDown.GetComponent<TextMesh>().text = down.ToString();
            BTDRight.GetComponent<TextMesh>().text = right.ToString();
            /*WTDUp.GetComponent<TextMesh>().text = up.ToString();
            WTDLeft.GetComponent<TextMesh>().text = left.ToString();
            WTDDown.GetComponent<TextMesh>().text = down.ToString();
            WTDRight.GetComponent<TextMesh>().text = right.ToString();*/

            BTDUp.transform.position = nowPosition + new Vector3(0, 0, 1);
            BTDLeft.transform.position = nowPosition + new Vector3(-1, 0, 0);
            BTDDown.transform.position = nowPosition + new Vector3(0, 0, -1);
            BTDRight.transform.position = nowPosition + new Vector3(1, 0, 0);
            /*WTDUp.transform.position = nowPosition + new Vector3(0, 0, 1);
            WTDLeft.transform.position = nowPosition + new Vector3(-1, 0, 0);
            WTDDown.transform.position = nowPosition + new Vector3(0, 0, -1);
            WTDRight.transform.position = nowPosition + new Vector3(1, 0, 0);*/



            Debug.Log(playerAvatar + "플레이어 주위의 폭탄의 갯수는" + "상" + up + "좌" + left + "하" + down + "우" + right + "입니다");
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