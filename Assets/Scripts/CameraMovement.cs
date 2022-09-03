using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;

    [SerializeField] private Transform _camTransform;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetDistance;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _aimSpeed;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        var targetPos = _target.transform.position + _targetDistance;
        //transform.position = Vector3.Lerp(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);

        var look = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
        transform.rotation = look; //Quaternion.RotateTowards(transform.rotation, look, _aimSpeed * Time.deltaTime);
    }

    public static void StartShake(float duration, float amount)
    {
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.Shake(duration, amount));
    }

    public IEnumerator Shake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (duration > 0)
        {
            _camTransform.localPosition = Random.insideUnitSphere * amount;

            duration -= Time.deltaTime;

            yield return null;
        }

        _camTransform.localPosition = Vector3.zero;
    }
}