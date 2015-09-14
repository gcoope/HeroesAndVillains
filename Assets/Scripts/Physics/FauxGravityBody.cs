using UnityEngine;
using System.Collections;

namespace smoothstudio.heroesandvillains.physics
{
    public class FauxGravityBody : MonoBehaviour
    {
        private float gravity = Settings.Gravity;

        private GameObject attractor;
        private FauxGravityAttractor currentAttractor;
        private FauxGravityAttractor[] gravityAttractors;

        private Transform attractorTransform;
        private Transform bodyTransform;
        protected Rigidbody bodyRigidbody;

        
        private float distToGround;
        protected Vector3 gravityUp;
        private Vector3 bodyUp;

        [SerializeField]protected bool bodyIsGrounded = false;

        public virtual void Awake() {
            gravityAttractors = GameObject.FindObjectsOfType<FauxGravityAttractor>();
        }

        void Start() {
            foreach(FauxGravityAttractor attr in gravityAttractors) {
                if(currentAttractor == null) {
                    currentAttractor = attr;
                } else {
                    if(attr.massValue > currentAttractor.massValue) {
                        currentAttractor = attr;
                    }
                    if (attr.forceAttract) currentAttractor = attr; // Override
                }
            }

            attractor = currentAttractor != null ? currentAttractor.gameObject : GameObject.Find("Planet");

            attractorTransform = attractor.GetComponent<Transform>();
            bodyRigidbody = gameObject.GetComponent<Rigidbody>();
            bodyTransform = gameObject.GetComponent<Transform>();

			if(bodyRigidbody == null) {
				bodyRigidbody = gameObject.AddComponent<Rigidbody>();
			}

            if (bodyRigidbody != null) {
                bodyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                bodyRigidbody.useGravity = false;
            }
        }

        void FixedUpdate() {
            Attract();
        }

//		void Update() {
//			Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), -gravityUp);
//		}

		void OnCollisionEnter(Collision col) {
			if(col.gameObject == attractor) {
				bodyIsGrounded = true;
			}
		}

		void OnCollisionExit(Collision col) {
			if(col.gameObject == attractor) {
				bodyIsGrounded = false;
			}
		}

        private void Attract() {
            gravityUp = (bodyTransform.position - attractorTransform.position).normalized;
            bodyUp = bodyTransform.up;
            this.bodyRigidbody.AddForce(gravityUp * gravity);

            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * bodyTransform.rotation;
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, targetRotation, 50 * Time.deltaTime);
        }
    }
}
