using UnityEngine;

namespace JoJosAdventure.Utils
{
    public class UtilClass
    {
        public static Vector3 DirFromAngleGlobal(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
        }

        public static Vector3 DirFromAngleLocal(float angleInDegrees, Transform transform)
        {
            angleInDegrees += GetGlobalTransformAngleAddition(transform);

            return DirFromAngleGlobal(angleInDegrees);
        }

        public static float GetGlobalTransformAngleAddition(Transform transform)
        {
            // this will also take into account rotation of parent
            return transform.eulerAngles.y
                + transform.eulerAngles.z;
        }
    }
}