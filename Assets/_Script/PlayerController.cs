using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Script
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public CharacterController controller;
        public float speed = 3.0F;
        public float rotateSpeed = 3.0F;
        public bool isMove = false;
        private Animator _ani;

        //debug
        public bool debug = true;

        //gun area
        public Transform gunBarrel;
        public GameObject GunParticles;
        public GameObject Lazer;

        public Camera Cam;
        private Vector3 movement;

        Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
        private int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
        private float camRayLength = 100f;          // The length of the ray from the camera into the scene.
        //sho0t
        public float shotTimer = 0f;
        public float shotCoolDown = .2f;

        //HP
        public int PlayerHP = 100;
        public bool isDead = false;
        //hpbar
        public Slider HPBarSlider;

        void Awake()
        {
            //GameObjectLoad();
            // Create a layer mask for the floor layer.
            floorMask = LayerMask.GetMask("Floor");
            // Set up references.
            //GameObject.FindGameObjectWithTag ("Player")
            _ani = GameObject.Find("PlayerModel").GetComponent<Animator>();
            //_ani = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
            //HPBar 
            HPBarSlider = GameObject.Find("HPBar").GetComponent<Slider>();
        }


        void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        void Update()
        {
            if (isDead) return;
            MoveController();
        }

        void FixedUpdate()
        {
            if (isDead)
                return;
            ApplyAnimation();
            //ApplyLook();
            ApplyShot();
        }

        void LateUpdate()
        {
            if (isDead) return;
            Turning();
        }
        private void ApplyShot()
        {
            shotTimer += Time.deltaTime;
            if (shotTimer < shotCoolDown) return;
            if (Input.GetAxis("Fire1").Equals(0)) return;
            shotTimer = 0f;
            GameObject obj = Instantiate(GunParticles, gunBarrel.transform.position, gunBarrel.rotation) as GameObject;
            obj.transform.parent = gunBarrel;
            Instantiate(Lazer, gunBarrel.position, gunBarrel.rotation);
            Destroy(obj, 1f);

            //Debug.Log("Shot");

        }

        void Turning()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(Input.mousePosition);
            //ve mot tia tu camera den' huong'
            Debug.DrawRay(Cam.transform.position, camRay.direction * 1000f, Color.yellow);
            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;
            // Perform the raycast and if it hits something on the floor layer...
            //gap floor thi dung lai
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;
                Debug.DrawRay(transform.position, playerToMouse, Color.cyan);
                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;
                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                // Set the player's rotation to this new rotation.
                playerRigidbody.MoveRotation(newRotation);
            }
        }
        public void ApplyLook()
        {
            //tao doi tuong va cham hit
            RaycastHit hit = new RaycastHit();
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);// tao tia tu Cam den chuot
            //Debug.DrawLine(Cam.transform.position, ray.GetPoint(1000), Color.red);
            if (Physics.Raycast(ray, out hit))
            {
                transform.LookAt(hit.point);
            }
            //Instantiate(particle, transform.position, transform.rotation);
        }

        void MoveController()
        {
            float hori = Input.GetAxis("Horizontal");
            float verti = Input.GetAxis("Vertical");
            isMove = false;
            if (hori != 0 || verti != 0) isMove = true;

            //transform.Rotate(0, hori * rotateSpeed, 0);
            //Vector3 forward = transform.TransformDirection(Vector3.forward);
            //float curSpeed = speed * verti;
            //controller.SimpleMove(forward * curSpeed);
            movement = new Vector3(hori, 0f, verti);
            controller.SimpleMove(movement * speed);
        }
        void ApplyAnimation()
        {
            _ani.SetBool("isMove", isMove);
        }

        public void PlayerHit(int dam)
        {
            if (isDead)
            {
                return;
            }
            PlayerHP -= dam;
            HPBarSlider.value = PlayerHP;
            if (PlayerHP < 1)
            {
                _ani.SetTrigger("Died");
                isDead = true;
            }
        }

        public void RestartLevel()
        {

        }

        void _debug(string text)
        {
            if (debug)
            {
                Debug.Log(" " + text);
            }
        }

    }
}
