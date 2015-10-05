using smoothstudio.heroesandvillains.player.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace smoothstudio.heroesandvillains.physics { 
    class PlayerGravityBody : FauxGravityBody {

		public bool localPlayer = false;

        public override void Awake() {
			base.Awake();
            gameObject.AddGlobalEventListener(PlayerEvent.PlayerJump, Jump);
        }

        private void Jump(EventObject evt) {
			if(!localPlayer) return;
            if(evt.Params[0] != null) {
            	bodyRigidbody.AddForce(gravityUp * (float)evt.Params[0], ForceMode.Impulse);
            }
        }	
    }
}
