using UnityEngine;
using UnityEditor;

namespace Goodfellow.Utils
{
    public class DropToFloor
    {
        /*
        These are the supported keys (can also be combined together):

        % – CTRL on Windows / CMD on OSX
        # – Shift
        & – Alt
        LEFT/RIGHT/UP/DOWN – Arrow keys
        F1…F2 – F keys
        HOME, END, PGUP, PGDN

        Character keys not part of a key-sequence are added by adding an underscore prefix to them (e.g: _g for shortcut key “G”).
        */

        [MenuItem("Edit/Drop to Floor _g")]
        private static void DoDropToFloor()
        {
            Transform active = Selection.activeTransform;
            if(active == null)
                return;

            Renderer renderer = active.GetComponent<Renderer>();
            if(renderer == null)
                return;

            Bounds bounds = renderer.bounds;

            RaycastHit[] hits;
            hits = Physics.BoxCastAll(bounds.center, bounds.extents, Vector3.down, Quaternion.identity, 100f);

            float highest = active.position.y - 100;
            foreach(RaycastHit hit in hits)
            {
                if(hit.transform == active)
                {
                    continue;
                }
                if(hit.point.y > highest)
                {
                    highest = hit.point.y;
                }
            }

            Undo.RecordObject(active, "Drop to Floor");
            active.position = new Vector3(active.position.x, highest, active.position.z);
            active.Translate(Vector3.up * renderer.bounds.extents.y);
        }
    }
}
