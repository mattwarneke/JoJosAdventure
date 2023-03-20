using JoJosAdventure.Enemies;
using JoJosAdventure.Utils;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fow = (FieldOfView)this.target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.right, 360, fow.viewRadius);
            Vector3 viewAngleA = UtilClass.DirFromAngleLocal(-fow.viewAngle / 2, fow.transform);
            Vector3 viewAngleB = UtilClass.DirFromAngleLocal(fow.viewAngle / 2, fow.transform);

            Handles.DrawLine(fow.transform.position, fow.transform.position + (viewAngleA * fow.viewRadius));
            Handles.DrawLine(fow.transform.position, fow.transform.position + (viewAngleB * fow.viewRadius));

            //Handles.color = Color.red;
            //foreach (Transform visibleTarget in fow.visibleTargets)
            //{
            //    Handles.DrawLine(fow.transform.position, visibleTarget.position);
            //}
        }
    }
}