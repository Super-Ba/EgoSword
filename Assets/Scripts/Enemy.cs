using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity;

    [SerializeField] private float _knockBackJumpPower;
    [SerializeField] private float _knockBackPushPower;

    protected Player target;

    private CharacterController _controller;

    private Vector3 _moveDir = Vector3.zero;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }


    public void init(Player target, int hp)
    {
        this.target = target;
        Hp = MaxHp = hp;
    }

    private void Update()
    {
        if(_controller.isGrounded)
        {
            var dir = (target.transform.position - transform.position);
            dir.y = 0;

            _moveDir = dir.normalized * _moveSpeed;
        }

        _moveDir.y -= _gravity;


        _controller.Move(_moveDir * Time.deltaTime);
    }


    protected override void Attack()
    {
        base.Attack();
    }

    public override void Damage(Vector3 hitSource, int power)
    {
        Debug.Log($"{Hp} {power}");
        var dir = (transform.position - target.transform.position);
        dir.y = 0;
        dir.Normalize();

        dir *= _knockBackPushPower;
        dir.y = _knockBackJumpPower;

        _controller.Move(dir);

        Hp -= power;

        if(Hp < 0)
        {
            Destroy(this.gameObject);
        }
    }

}
