using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetDistance;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _aimSpeed;

    // Update is called once per frame
    void Update()
    {
        var targetPos = _target.transform.position + _targetDistance;
        //transform.position = Vector3.Lerp(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        var look = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
        transform.rotation = look;//Quaternion.RotateTowards(transform.rotation, look, _aimSpeed * Time.deltaTime);
    }
}
