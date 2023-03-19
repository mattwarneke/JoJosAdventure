using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    ///[HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    private Mesh mesh;

    public bool targetAquired = false;

    private void Start()
    {
        this.mesh = new Mesh();
        this.mesh.name = "View Mesh";
        this.viewMeshFilter.mesh = this.mesh;

        this.StartCoroutine("FindTargetsWithDelay", .2f);
    }

    private void LateUpdate()
    {
        this.DrawFieldOfViewImproved();
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            this.FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        this.visibleTargets.Clear();
        this.targetAquired = false;
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), this.viewRadius, this.targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - this.transform.position).normalized;
            if (Vector2.Angle(new Vector2(Mathf.Sin(this.getGlobalAngleAddition() * Mathf.Deg2Rad), Mathf.Cos(this.getGlobalAngleAddition() * Mathf.Deg2Rad)), dirToTarget) < this.viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(this.transform.position, target.position);

                if (!Physics2D.Raycast(this.transform.position, dirToTarget, dstToTarget, this.obstacleMask))
                {
                    this.visibleTargets.Add(target);
                    this.targetAquired = true;
                }
            }
        }
    }

    // cache arrays for performance
    private Vector3[] vertices;

    private int[] triangles;
    private List<Vector3> viewPoints;

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(this.viewAngle * this.meshResolution);
        float stepAngleSize = this.viewAngle / stepCount;
        this.viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; i++)
        {
            // Need to factor in the z of the transform + the body direction
            float angle = this.getGlobalAngleAddition() - (this.viewAngle / 2) + (stepAngleSize * i);
            // global Angle since we already added the rotation in with the subtraction
            //Vector3 dir = DirFromAngle(angle, true);
            //Debug.DrawLine(transform.position, transform.position + dir * viewAngle, Color.red);

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
            float angle = this.getGlobalAngleAddition() - (this.viewAngle / 2) + (stepAngleSize * i);
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
        Vector3 dir = this.DirFromAngle(globalAngle, true);
        RaycastHit2D hit;

        // DEBUG:
        //Ray testRay = new Ray(this.transform.position, dir * viewRadius);
        //Debug.DrawRay(testRay.origin, testRay.direction * viewRadius);

        hit = Physics2D.Raycast(this.transform.position, dir, this.viewRadius);//, obstacleMask);
        if (hit.collider != null)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, this.transform.position + (dir * this.viewRadius), this.viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += this.getGlobalAngleAddition();
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private float getGlobalAngleAddition()
    {
        // this will also take into account rotation of parent
        return this.transform.eulerAngles.y
            + this.transform.eulerAngles.z;
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