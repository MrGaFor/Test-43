using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform Target;
    [SerializeField] private float _speed = 15f;

    private void Start()
    {
        if (!Target)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, _speed * Time.deltaTime);
    }

    /*void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, _speed * Time.deltaTime);
    }*/
}
