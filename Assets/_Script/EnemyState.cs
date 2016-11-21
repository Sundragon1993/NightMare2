using UnityEngine;

namespace Assets._Script
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class EnemyState : MonoBehaviour
    {
        public int gameScore;

        public float sinkSpeed = 2.5f;
        private bool isSinking;

        public Transform player;
        public NavMeshAgent nav;
        public Animator ani;

        public bool isDead = false;
        private bool debug = true;

        //máu 
        public int CurrentHP = 5;
        public int Damage = 2;
        //attack
        public float AttackCoolDown = 1;
        public float AttackTimer;

        // Use this for initialization
        void Start()
        {
            GameObjectLoad();
        }

        private void GameObjectLoad()
        {
            player = GameObject.Find("Player").transform;
            nav = GetComponent<NavMeshAgent>();
            ani = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
            if (isSinking)
            {
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
            }
            FindPlayer();
            // EnemyCheckDead();
        }

        private void FindPlayer()
        {
            if (player.GetComponent<PlayerController>().isDead)
            {
                ani.SetTrigger("PlayerDeath");
                nav.enabled = false;
                return;
            }
            if (player == null || isDead) return;
            nav.SetDestination(player.position);
        }

        public void EnemyHit()
        {
            if (isDead)
                return;

            CurrentHP--;
            if (CurrentHP > 0) return;

            isDead = true;
            ani.SetTrigger("Dead");
            if (isDead)
            {
                //player.GetComponent<GameController>().gameScore = 1;
                gameScore += 1;
                transform.tag = "EnemyDead";
                GetComponent<AudioSource>().Play();
                nav.enabled = false;
            }
        }

        void OnTriggerStay(Collider other)
        {
            //_debug("other" + other.name);
            //_debug("transform " + transform.name);
            if (other.tag != "Player") return;
            EnemyAttack1(other.gameObject);
        }

        private void EnemyAttack1(GameObject o)
        {
            //neu nhan vat chet roi thi khong danh nua
            if (player.GetComponent<PlayerController>().isDead) return;
            //con song thi lam nhung chuyen o duoi
            AttackTimer += Time.deltaTime;
            if (AttackTimer < AttackCoolDown) return;
            AttackTimer = 0;
            o.GetComponent<PlayerController>().PlayerHit(Damage);
        }

        void _debug(string text)
        {
            if (debug)
            {
                Debug.Log(" " + text);
            }
        }


        //tao event trong animation cua zombunny
        void EnemyDead()
        {
            nav.enabled = false;
            ani.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            isSinking = true;
            Destroy(transform.gameObject, 2f);
        }
    }
}
