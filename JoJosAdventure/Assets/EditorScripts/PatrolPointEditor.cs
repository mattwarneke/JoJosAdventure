using JoJosAdventure.Common;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    [CustomEditor(typeof(PatrolPoint))]
    public class PatrolPointEditor : Editor
    {
        private void OnSceneGUI()
        {
            PatrolPoint point = (PatrolPoint)this.target;

            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(point.transform.position, new Vector3(0, 0, 1), 0.1f);
        }
    }
}