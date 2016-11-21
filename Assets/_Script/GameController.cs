using UnityEngine;
using UnityEngine.UI;

namespace Assets._Script
{
    public class GameController : MonoBehaviour
    {

        

        public GameObject[] spawnPoint;
        public GameObject[] numZombies;
        public float spawnDelays = 3f;
        public float spawn_call_delay = 1f;

        public bool debug = true;
        // Use this for initialization
        public Transform player;

        public Text gameOver;
        public Text _Score;
        void Start()
        {
            InvokeRepeating("SpawnZombie", spawn_call_delay, spawnDelays);
        }

        void Update()
        {
            GameOverCheck();
            GameScoreCheck();

        }

        private void GameScoreCheck()
        {
            //_Score.text = "Score: " + gamesc;
        }

        private void GameOverCheck()
        {
            if (player.GetComponent<PlayerController>().isDead == false) return;
            if (gameOver.enabled == false)
                gameOver.enabled = true;
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }

        void Awake()
        {
            GameObjectLoad();
        }

        private void GameObjectLoad()
        {
            player = GameObject.Find("Player").transform;
            //int gamesc = player.GetComponent<EnemyState>().gameScore;
        }

        void SpawnZombie()
        {
            if (player.GetComponent<PlayerController>().isDead) return;
            //_debug("SpawnZombie");
            GameObject zombie;
            zombie = Zombie_Load();
            GameObject spawn_point;
            spawn_point = SpawnPoint_Load();
            Instantiate(zombie, spawn_point.transform.position, Quaternion.identity);
        }
        //load diem ra zombie
        private GameObject SpawnPoint_Load()
        {
            int index = Random.Range(0, spawnPoint.Length);
            return spawnPoint[index];
        }

        //load zombie
        GameObject Zombie_Load()
        {
            int index = Random.Range(0, numZombies.Length);
            return numZombies[index];
        }


        /// <summary>
        /// Show specified debug text
        /// </summary>
        /// <param name="text"></param>
        void _debug(string text)
        {
            if (debug)
                Debug.Log("GameController: " + text);
        }
    }
}
