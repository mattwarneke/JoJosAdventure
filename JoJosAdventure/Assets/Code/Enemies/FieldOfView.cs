using JoJosAdventure.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace JoJosAdventure.Enemies
{
    public class FieldOfView : MonoBehaviour
    {
        /// <summary>
        /// The angle width of this field of view
        /// </summary>
        public float viewAngle = 40f;

        /// <summary>
        /// The distance the field of view travels
        /// </summary>
        public float viewRadius = 5f;

        /// <summary>
        /// How many rays per angle
        /// </summary>
        public float meshResolution = 0.25f;

        /// <summary>
        /// Must include playerLayer to detect in ray collision
        /// </summary>
        public LayerMask rayMask;

        public int edgeResolveIterations;
        public float edgeDstThreshold;

        public MeshFilter viewMeshFilter;
        private Mesh mesh;

        private bool PlayerInSight = false;
        private Quaternion initialRotation;

        private void Start()
        {
            this.mesh = new Mesh();
            this.mesh.name = "View Mesh";
            this.viewMeshFilter.mesh = this.mesh;
            this.initialRotation = this.transform.rotation;
        }

        public bool IsPlayerInFieldOfView()
        {
            return this.PlayerInSight;
        }

        public bool LookForPlayer(Collider2D targetCollider)
        {
            this.PlayerInSight = false;

            this.DrawFieldOfView(targetCollider);

            return this.PlayerInSight;
        }

        // cache arrays for performance
        private Vector3[] vertices;

        private int[] triangles;
        private List<Vector3> viewPoints = new List<Vector3>();

        private void DrawFieldOfView(Collider2D targetCollider)
        {
            int stepCount = Mathf.RoundToInt(this.viewAngle * this.meshResolution);
            float stepAngleSize = this.viewAngle / stepCount;
            this.viewPoints.Clear();
            // ray cast in a cone shape - look for collisions and player
            for (int i = 0; i <= stepCount; i++)
            {
                float angle = UtilClass.GetGlobalTransformAngleAddition(this.transform) - (this.viewAngle / 2) + (stepAngleSize * i);

                ViewCastInfo newViewCast = this.ViewCast(angle, targetCollider);
                this.viewPoints.Add(newViewCast.point);
            }

            int vertexCount = this.viewPoints.Count + 1;
            this.vertices = new Vector3[vertexCount];
            this.triangles = new int[(vertexCount - 2) * 3];

            // create the mesh vertices to represent the field of view
            this.vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                this.vertices[i + 1] = this.transform.InverseTransformPoint(this.viewPoints[i]);

                if (i < vertexCount - 2)
                {
                    this.triangles[i * 3] = 0;
                    this.triangles[(i * 3) + 1] = i + 1;
                    this.triangles[(i * 3) + 2] = i + 2;
                }
            }

            this.mesh.Clear();

            this.mesh.vertices = this.vertices;
            this.mesh.triangles = this.triangles;
            this.mesh.RecalculateNormals();
        }

        public void RotateFOVTowardsTarget(Vector3 target)
        {
            Vector2 direction = target - this.transform.position;
            // We must flip the y for Unity, y points upwards, rather than down as the Mathf would calculate
            direction.y = -direction.y;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // maintain intial rotation
            angle -= this.initialRotation.eulerAngles.z;

            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // USE THE BELOW SCRIPT IF THE PARENT OBJECT ALSO ROTATES
            //// Get the initial rotation of the game object in world space
            //Quaternion initialWorldRotation = Quaternion.Euler(0f, 0f, this.initialRotation.eulerAngles.z) * this.transform.parent.rotation;

            //// Construct a new rotation that only updates the Z-axis
            //Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //// Apply the new rotation to the game object's local rotation, while maintaining the initial world rotation
            //this.transform.rotation = initialWorldRotation * newRotation;
        }

        private ViewCastInfo ViewCast(float globalAngle, Collider2D targetCollider)
        {
            Vector3 dir = UtilClass.DirFromAngleGlobal(globalAngle);
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, this.viewRadius, this.rayMask);
            if (hit.collider != null)
            {
                if (hit.collider.Equals(targetCollider))
                {
                    this.PlayerInSight = true;
                }

                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, this.transform.position + (dir * this.viewRadius), this.viewRadius, globalAngle);
            }
        }

        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
            {
                this.hit = _hit;
                this.point = _point;
                this.dst = _dst;
                this.angle = _angle;
            }
        }
    }
}