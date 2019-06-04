using System.Collections;
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
      
       
        void OnTriggerStay(Collider other) //박스 & 지뢰 습득 스크립트
        {
            if (other.gameObject.CompareTag("Box")&&Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(ReviveBox(other.gameObject, BoxSpawnTime));
            }
            else if (other.gameObject.CompareTag("Bomb") && Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(ReviveBomb(other.gameObject, BombSpawnTime));
                Explosion((int)transform.position.x,(int)transform.position.z);
                Instantiate(ExplosionPrefab, transform.position, transform.rotation);

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

        public IEnumerator ReviveBox(GameObject box, float time)
        {
            box.SetActive(false);
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            box.SetActive(false);
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