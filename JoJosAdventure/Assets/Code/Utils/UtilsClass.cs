using UnityEngine;

namespace Assets.Code
{
    public class UtilsClass
    {
        public static Vector3 GetVectorFromAngle(float angle)
        {   // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
    }
}