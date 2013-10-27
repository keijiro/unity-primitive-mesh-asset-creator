using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour
{
    void Update ()
    {
        transform.localRotation =
            Quaternion.AngleAxis (Time.time * 120.0f, Vector3.up) *
            Quaternion.AngleAxis (Time.time * 33.7f, Vector3.right);
    }
}