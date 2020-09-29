using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper3Dto2D : MonoBehaviour
{
    static Material lineMaterial;
    public int type = 1;
    Color color;

    private void Start()
    {
        if (type == 1)
        {
            color = Color.white;
        }
        else if (type == 2)
        {
            color = Color.cyan;
        }
        else if (type == 3)
        {
            color = Color.red;
        }
    }

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

    Vector3 topLeft = Vector3.zero, topRight = Vector3.zero, bottomLeft = Vector3.zero, bottomRight = Vector3.zero;

    public void GetProjectionPoints(int count,Vector3 point)
    {
        if (count == 1)
        {
            topLeft = point;
        }
        else if (count == 2)
        {
            topRight = point;
        }
        else if (count == 3)
        {
            bottomLeft = point;
        }
        else if (count == 4)
        {
            bottomRight = point;
        }
    }

    void OnRenderObject()
    {
        if (topLeft == Vector3.zero || topRight == Vector3.zero || bottomLeft == Vector3.zero || bottomRight == Vector3.zero)
            return;

        CreateLineMaterial();

        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.QUADS);
        GL.Color(color);

        GL.Vertex3(topLeft.x,topLeft.y,topLeft.z);
        GL.Vertex3(bottomLeft.x,bottomLeft.y,bottomLeft.z);
        GL.Vertex3(bottomRight.x,bottomRight.y,bottomRight.z);
        GL.Vertex3(topRight.x, topRight.y, topRight.z);

        GL.End();
        GL.PopMatrix();
    }
}
