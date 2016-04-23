﻿using smoothstudio.heroesandvillains.player.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace smoothstudio.heroesandvillains.physics { 
    class PlayerGravityBody : FauxGravityBody {

		public void Setup(Rigidbody rBody) {
			bodyRigidbody = rBody;
			if(applyUprightRotation) bodyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			bodyRigidbody.useGravity = false;
			bodyRigidbody.drag = 0.5f;
			bodyRigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
			bodyRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}

        public void Jump(float jumpPower) {
			if (bodyRigidbody != null) {
				bodyRigidbody.AddForce (gravityUp * jumpPower, ForceMode.Impulse);
			}
        }	

    }
}
