using UnityEngine;
using System.Collections;

namespace smoothstudio.heroesandvillians.player
{
    public class PlayerMove : MonoBehaviour
    {

        public Camera playerCamera;
        private float moveSpeed = 15f;
        private float rotateSpeed = 20f;
        private Vector3 moveDir;
        private Rigidbody playerRigidbody;
        //private Transform playerTransform;

        void Awake() { 
            playerRigidbody = GetComponent<Rigidbody>();
            if (playerRigidbody == null) playerRigidbody = gameObject.AddComponent<Rigidbody>();
           // playerTransform = GetComponent<Transform>();
        }

        void Update() {
            RecieveInput();
        }

        private void RecieveInput() {
            moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical")).normalized;
            if (Input.GetAxisRaw("Horizontal") < 0) transform.Rotate(0, -5, 0 * Time.deltaTime * rotateSpeed); // L
            if (Input.GetAxisRaw("Horizontal") > 0) transform.Rotate(0, 5, 0 * Time.deltaTime * rotateSpeed); // R
        }

        void FixedUpdate() {
            this.playerRigidbody.MovePosition(playerRigidbody.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
        }
    }
}