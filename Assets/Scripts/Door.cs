using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private GameObject _doorObject;
    [SerializeField] private Transform _destination;
    [SerializeField] private Transform _enemyParent;


    void Update()
    {
        _collider.enabled = _enemyParent.childCount <= 0;
        _doorObject.SetActive(_enemyParent.childCount <= 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //캐릭터 컨트롤러 떄문에 활성화한채로 움직이면 안움직여짐 ㅋㅋ
            other.gameObject.SetActive(false);
            other.transform.position = _destination.transform.position;
            other.gameObject.SetActive(true);
        }
    }
}