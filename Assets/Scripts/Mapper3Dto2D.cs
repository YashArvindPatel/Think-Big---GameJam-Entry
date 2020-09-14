using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper3Dto2D : MonoBehaviour
{
    static Material lineMaterial; // Put in the Line material of choice

    // Once per collision call while drawing in World Space

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    Vector3[] points = new Vector3[4];
    bool printOK = false;

    // Called every frame a collision happens

    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            // Store all the 4 closest points to the edge collision points in the array

            points[i] =  (collision.contacts[i].point);
            Debug.DrawRay(points[i], Vector3.forward, Color.cyan, 1); // Used for visualizing points from rays
        }
        printOK = true;
    }
    
    // Called every frame after MainCamera is done rendering everything

    void OnRenderObject()
    {
        if (!printOK) // Check if collision is true, depending on that render the 2D Shape
            return;

        // Create a 2D Shape from the points calculated to represent the 3D Shape

        CreateLineMaterial();

        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.QUADS);
        GL.Color(Color.red);

        GL.Vertex3(points[0].x, points[0].y, points[0].z);
        GL.Vertex3(points[1].x, points[1].y, points[1].z);
        GL.Vertex3(points[2].x, points[2].y, points[2].z);
        GL.Vertex3(points[3].x, points[3].y, points[3].z);

        GL.Vertex3(points[3].x, points[3].y, points[3].z);
        GL.Vertex3(points[0].x, points[0].y, points[0].z);
        GL.Vertex3(points[1].x, points[1].y, points[1].z);
        GL.Vertex3(points[2].x, points[2].y, points[2].z);

        GL.Vertex3(points[2].x, points[2].y, points[2].z);
        GL.Vertex3(points[3].x, points[3].y, points[3].z);
        GL.Vertex3(points[0].x, points[0].y, points[0].z);
        GL.Vertex3(points[1].x, points[1].y, points[1].z);

        GL.Vertex3(points[1].x, points[1].y, points[1].z);
        GL.Vertex3(points[2].x, points[2].y, points[2].z);
        GL.Vertex3(points[3].x, points[3].y, points[3].z);
        GL.Vertex3(points[0].x, points[0].y, points[0].z);

        GL.End();
        GL.PopMatrix();
    }
}
