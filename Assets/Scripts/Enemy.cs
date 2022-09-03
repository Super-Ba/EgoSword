using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Actor
{
    [SerializeField] private Image _hpGuage;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity;

    [SerializeField] private float _knockBackJumpPower;
    [SerializeField] private float _knockBackPushPower;
    [SerializeField] private float _knockBackCoolTime;

    protected Player target;

    private CharacterController _controller;

    private Vector3 _moveDir = Vector3.zero;

    private bool _isKnockBack = false;
    private Vector3 _knockBackForce;
    private float _lastKnockBackTime;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void init(Player target, int hp)
    {
        this.target = target;
        Hp = MaxHp = hp;

        _hpGuage.fillAmount = (float) Hp / MaxHp;
    }

    private void Update()
    {
        if (!_isKnockBack)
        {
            if (_controller.isGrounded)
            {
                var dir = (target.transform.position - transform.position);
                dir.y = 0;

                _moveDir = dir.normalized * _moveSpeed;
            }

            _moveDir.y -= _gravity;

            _controller.Move(_moveDir * Time.deltaTime);
        }
        else
        {
            _controller.Move((_knockBackForce + Vector3.down * _gravity) * Time.deltaTime);
            _knockBackForce = Vector3.Lerp(_knockBackForce, Vector3.zero, 5 * Time.deltaTime);

            if (_lastKnockBackTime + _knockBackCoolTime < Time.time)
            {
                _isKnockBack = false;
            }
        }
    }

    protected override void Attack()
    {
        base.Attack();
    }

    public override void Damage(Vector3 hitSource, int power)
    {
        Debug.Log($"{Hp} {power}");
        _knockBackForce = (transform.position - target.transform.position);
        _knockBackForce.y = 0;
        _knockBackForce.Normalize();

        _knockBackForce *= _knockBackPushPower;
        _knockBackForce.y = _knockBackJumpPower;

        _isKnockBack = true;
        _lastKnockBackTime = Time.time;

        Hp -= power;

        _hpGuage.fillAmount = (float) Hp / MaxHp;

        if (Hp < 0)
        {
            Destroy(this.gameObject);
        }
    }
}