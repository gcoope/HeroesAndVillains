using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    private static float gravity = -15f;

    public static float Gravity {
        get {
            return gravity;
        }

        private set {
            //
        }
    }

}
