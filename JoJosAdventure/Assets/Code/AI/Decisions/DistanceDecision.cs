using UnityEngine;

namespace JoJosAdventure
{
    public class DistanceDecision : AIDecision
    {
        [field: SerializeField]
        [field: Range(0.1f, 10)]
        public float Distance { get; set; } = 5f;

        public override bool MakeADecision()
        {
            // The target of AI system vs ourselves
            if (Vector3.Distance(this.EnemyBrain.Target.transform.position, this.transform.position) < this.Distance)
            {
                this.AIActionData.Attack = true;
            }
            else
            {
                this.AIActionData.Attack = false;
            }
            return this.AIActionData.TargetSpotted;
        }

        // for editor debug
        protected void OnDrawGizmos()
        {
            if (UnityEditor.Selection.activeObject == this.gameObject)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(this.transform.position, this.Distance);
                Gizmos.color = Color.white;//reset gizmo color afterwards for other gizmos
            }
        }
    }
}