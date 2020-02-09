using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Camera camera;

    float speed = 1;

    void LateUpdate()
    {
        if(Input.GetKey(KeyCode.Q)) 
            Move();
    }

    void Move()
    {
        Vector3 direction = (target.position - camera.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        lookRotation.x = transform.rotation.x;
        lookRotation.z = transform.rotation.z;

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100);
        transform.position = Vector3.Slerp(transform.position, target.position, Time.deltaTime * speed);
    }
}
