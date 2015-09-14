using UnityEngine;
using System.Collections;

namespace smoothstudio.heroesandvillians.physics
{
    public class FauxGravityAttractor : MonoBehaviour
    {
        public float massValue = 1f;
        public bool forceAttract = false; // Override all mass value checks
    }
}
