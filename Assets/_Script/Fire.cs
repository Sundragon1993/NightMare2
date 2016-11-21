using UnityEngine;

namespace Assets._Script
{
    public class Fire : MonoBehaviour
    {
        public float speed = 1000f;
        private Rigidbody _rb;
        public GameObject hitParticle;
        // Use this for initialization
        void Awake()
        {
            GameObjectLoad();
        }

        private void GameObjectLoad()
        {
            //tu load khong can keo tha
            hitParticle = Resources.Load("FireImpact") as GameObject;
        }


        void OnCollisionEnter(Collision collision)
        {
            Destroy(transform.gameObject);
            //Vector3 hit = other.ClosestPointOnBounds(transform.position);
            //Vector3 hitPoint = collision.contacts.Length as Vector3;
            ContactPoint contactPoint = collision.contacts[0];
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
            Vector3 pos = contactPoint.point;


            GameObject fireImpact = Instantiate(hitParticle, pos, transform.rotation) as GameObject;
            Destroy(fireImpact, 4f);
        }
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            //Vector3 dir = transform.position;
            _rb.AddForce(transform.forward * speed);
            Destroy(transform.gameObject, 8f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Shootable") return;
            //tao hitparticle
            Vector3 hit = other.ClosestPointOnBounds(transform.position);
            GameObject fireImpact = Instantiate(hitParticle, hit, transform.rotation) as GameObject;
            Destroy(fireImpact, 4f);
            //destroy particle


            //destroy tia dan.
            Destroy(transform.gameObject);
            //AudioSource audio = other.GetComponent<AudioSource>();
            //audio.Play();
            //Destroy(other.gameObject);

            //set trigger cho zombie chet
            other.GetComponent<EnemyState>().EnemyHit();
        }
    }
}
