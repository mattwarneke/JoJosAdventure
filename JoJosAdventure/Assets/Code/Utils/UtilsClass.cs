using UnityEngine;

namespace JoJosAdventure.Utils
{
    public class UtilsClass
    {
        public static Vector3 DirFromAngleGlobal(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
        }

        public static Vector3 DirFromAngleLocal(float angleInDegrees, Transform transform)
        {
            angleInDegrees += GetGlobalAngleAddition(transform);

            return DirFromAngleGlobal(angleInDegrees);
        }

        public static float GetGlobalAngleAddition(Transform transform)
        {
            // this will also take into account rotation of parent
            return transform.eulerAngles.y
                + transform.eulerAngles.z;
        }
    }
}