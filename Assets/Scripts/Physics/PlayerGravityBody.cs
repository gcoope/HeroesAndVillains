using smoothstudio.heroesandvillians.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace smoothstudio.heroesandvillians.physics { 
    class PlayerGravityBody : FauxGravityBody {

        void Awake() {
			base.Awake();
            gameObject.AddGlobalEventListener(PlayerMoveEvent.PlayerJump, Jump);
        }

        private void Jump(EventObject evt) {
            if(evt.Params[0] != null) {
                if (bodyIsGrounded) {
                    bodyRigidbody.AddForce(gravityUp * 10, ForceMode.Impulse);
                }
            }
        }
    }
}
