using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject head;
    void Update()
    {
        transform.LookAt(head.transform);
    }
}
