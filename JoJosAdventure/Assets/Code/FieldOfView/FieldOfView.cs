using Assets.Code;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    /// <summary>
    /// The angle width of this field of view
    /// </summary>
    public float fieldOfView = 40f;

    /// <summary>
    /// The distance the field of view travels
    /// </summary>
    public float viewDistance = 5f;

    /// <summary>
    /// The amount of rays sent, this creats a nice curve the more rays but impacts perf
    /// </summary>
    private int rayCount = 25;

    private Mesh mesh;

    // cache arrays for performance
    private Vector3[] vertices;

    private Vector2[] uv;
    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        float currentAngle = 0f;
        float angleIncrease = fieldOfView / rayCount;

        // VERTICES ARE IN LOCAL SPACE
        // mesh origin is at this transform's position
        Vector3 origin = Vector3.zero;
        // positioning of points
        vertices = new Vector3[rayCount + 1 + 1];
        // texture rendered - vector 2 as the image it references is flat 2d
        uv = new Vector2[vertices.Length];
        // actual points of the mesh
        triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1; // 0 is the origin
        int triangleIndex = 0;
        for (int i = 1; i <= rayCount; i++)
        {
            Vector3 vertex;

            //Ray2D testRay = new Ray2D(transform.position, -UtilsClass.GetVectorFromAngle(currentAngle) * viewDistance);
            //Debug.DrawRay(testRay.origin, testRay.direction * 10);

            // RAYCAST MUST BE IN WORLD SPACE i.e transform.position
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, -transform.TransformDirection(UtilsClass.GetVectorFromAngle(currentAngle)), viewDistance);

            if (raycastHit2D.collider == null)
            {   // no collision make a cone using mesh
                vertex = origin + -UtilsClass.GetVectorFromAngle(currentAngle) * viewDistance;
            }
            else
            {
                //Debug.DrawRay(transform.position, (Vector3)raycastHit2D.point - transform.position, Color.green);
                vertex = (Vector3)raycastHit2D.point - transform.position;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            // goes counter clockwise if +, - for anti clockwise
            currentAngle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}