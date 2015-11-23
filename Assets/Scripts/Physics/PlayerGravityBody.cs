using smoothstudio.heroesandvillains.player.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace smoothstudio.heroesandvillains.physics { 
    class PlayerGravityBody : FauxGravityBody {
        public void Jump(float jumpPower) {
            bodyRigidbody.AddForce(gravityUp * jumpPower, ForceMode.Impulse);
        }	
    }
}
