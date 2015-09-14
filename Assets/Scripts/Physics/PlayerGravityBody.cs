using smoothstudio.heroesandvillians.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace smoothstudio.heroesandvillians.physics { 
    class PlayerGravityBody : FauxGravityBody {

		public bool localPlayer = false;

        void Awake() {
			base.Awake();
            gameObject.AddGlobalEventListener(PlayerMoveEvent.PlayerJump, Jump);
        }

        private void Jump(EventObject evt) {
			if(!localPlayer) return;
            if(evt.Params[0] != null) {
                if (bodyIsGrounded) {
                    bodyRigidbody.AddForce(gravityUp * (float)evt.Params[0], ForceMode.Impulse);
                }
            }
        }
    }
}
