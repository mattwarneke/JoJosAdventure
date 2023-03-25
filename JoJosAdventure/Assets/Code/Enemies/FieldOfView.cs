using JoJosAdventure.Utils;
using System;
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

        public event Action<Transform> OnPlayerSpotted;

        public bool PlayerInSight = false;
        private Transform PlayerTransform = null;
        private Quaternion initialRotation;

        private void Start()
        {
            this.mesh = new Mesh();
            this.mesh.name = "View Mesh";
            this.viewMeshFilter.mesh = this.mesh;
            this.initialRotation = this.transform.rotation;
        }

        private void LateUpdate()
        {
            this.DrawFieldOfView();

            if (this.PlayerInSight)
            {
                this.RotateIntoDirectionOfPlayer();
            }
        }

        private void RotateIntoDirectionOfPlayer()
        {
            Vector2 direction = this.PlayerTransform.position - this.transform.position;
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

        // cache arrays for performance
        private Vector3[] vertices;

        private int[] triangles;
        private List<Vector3> viewPoints;

        private void DrawFieldOfView()
        {
            this.PlayerInSight = false;

            int stepCount = Mathf.RoundToInt(this.viewAngle * this.meshResolution);
            float stepAngleSize = this.viewAngle / stepCount;
            this.viewPoints = new List<Vector3>();
            for (int i = 0; i <= stepCount; i++)
            {
                float angle = UtilClass.GetGlobalTransformAngleAddition(this.transform) - (this.viewAngle / 2) + (stepAngleSize * i);

                ViewCastInfo newViewCast = this.ViewCast(angle);
                this.viewPoints.Add(newViewCast.point);
            }

            int vertexCount = this.viewPoints.Count + 1;
            this.vertices = new Vector3[vertexCount];
            this.triangles = new int[(vertexCount - 2) * 3];

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

        private void DrawFieldOfViewImproved()
        {
            int rayCount = Mathf.RoundToInt(this.viewAngle * this.meshResolution);
            float stepAngleSize = this.viewAngle / rayCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();
            for (int i = 0; i < rayCount; i++)
            {
                float angle = UtilClass.GetGlobalTransformAngleAddition(this.transform) - (this.viewAngle / 2) + (stepAngleSize * i);
                ViewCastInfo newViewCast = this.ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > this.edgeDstThreshold;
                    if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                    {
                        EdgeInfo edge = this.FindEdge(oldViewCast, newViewCast);
                        if (edge.pointA != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointA);
                        }
                        if (edge.pointB != Vector3.zero)
                        {
                            viewPoints.Add(edge.pointB);
                        }
                    }
                }

                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = this.transform.InverseTransformPoint(viewPoints[i]);

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[(i * 3) + 1] = i + 1;
                    triangles[(i * 3) + 2] = i + 2;
                }
            }

            this.mesh.Clear();
            this.mesh.vertices = vertices;
            this.mesh.triangles = triangles;
            this.mesh.RecalculateNormals();
        }

        private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < this.edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = this.ViewCast(angle);

                bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > this.edgeDstThreshold;
                if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        private ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = UtilClass.DirFromAngleGlobal(globalAngle);
            RaycastHit2D hit;

            if (FlagsUtil.DebugMode)
            {
                Ray testRay = new Ray(this.transform.position, dir * this.viewRadius);
                Debug.DrawRay(testRay.origin, testRay.direction * this.viewRadius);
            }

            hit = Physics2D.Raycast(this.transform.position, dir, this.viewRadius, this.rayMask);
            if (hit.collider != null)
            {
                if (LayersUtil.IsColliderPlayer(hit.collider))
                {
                    this.OnPlayerSpotted.Invoke(hit.transform);
                    this.PlayerInSight = true;
                    this.PlayerTransform = hit.transform;
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

        public struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                this.pointA = _pointA;
                this.pointB = _pointB;
            }
        }
    }
}