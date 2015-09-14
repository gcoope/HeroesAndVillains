﻿using UnityEngine;
using System.Collections;

public class FauxGravityBody : MonoBehaviour {

	private float gravity = -15f;

	private GameObject attractor;
	private Transform attractorTransform;
	private Transform bodyTransform;
	private Rigidbody bodyRigidbody;
	private Collider bodyCollider;

	private LayerMask bodyLayer;

	private float distToGround;
	private Vector3 gravityUp;
	private Vector3 bodyUp;

	private bool bodyIsGrounded = false;

	void Start () {
		attractor = GameObject.Find("Planet");

		attractorTransform = attractor.GetComponent<Transform>();
		bodyRigidbody = gameObject.GetComponent<Rigidbody>();
		bodyTransform = gameObject.GetComponent<Transform>();
		bodyCollider = gameObject.GetComponent<Collider>();
		bodyLayer = gameObject.layer;

		if(bodyRigidbody != null) {
			bodyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			bodyRigidbody.useGravity = false;
		}
	}

	void FixedUpdate () {
		Attract();
	}

	private void Attract() {
		gravityUp = (bodyTransform.position - attractorTransform.position).normalized;
		bodyUp = bodyTransform.up;
		bodyIsGrounded = Physics.Raycast(transform.position, -gravityUp, 0.1f);
		
		if(Input.GetKeyDown(KeyCode.Space) && bodyIsGrounded) {
			bodyRigidbody.AddForce(gravityUp * 10, ForceMode.Impulse);
		}
		
		this.bodyRigidbody.AddForce(gravityUp * gravity);
		
		Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * bodyTransform.rotation;
		bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, targetRotation, 50 * Time.deltaTime);
	}
}
