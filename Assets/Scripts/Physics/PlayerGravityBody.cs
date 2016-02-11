using smoothstudio.heroesandvillains.player.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace smoothstudio.heroesandvillains.physics { 
    class PlayerGravityBody : FauxGravityBody {
        public void Jump(float jumpPower) {
			if(bodyRigidbody != null) bodyRigidbody.AddForce(gravityUp * jumpPower, ForceMode.Impulse);
        }	

		public void AddExplosionForce(Vector3 fromPosition, float power) {
//			bodyRigidbody.AddForceAtPosition((transform.position - fromPosition).normalized * 20f, fromPosition, ForceMode.Impulse);
			if(bodyRigidbody != null) bodyRigidbody.AddForce((transform.position - fromPosition).normalized * power, ForceMode.Impulse);
		}

    }
}
