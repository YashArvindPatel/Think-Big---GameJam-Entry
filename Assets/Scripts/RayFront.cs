using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayFront : MonoBehaviour
{
    RaycastHit hit;
    public Mapper3Dto2D mapper;
    public int count = 1;

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, transform.parent.lossyScale.z))
        {
            Debug.DrawLine(transform.position, hit.point);

            mapper.GetProjectionPoints(count, hit.point);
        }
        else
        {
            mapper.GetProjectionPoints(count, Vector3.zero);
        }
    }
}
