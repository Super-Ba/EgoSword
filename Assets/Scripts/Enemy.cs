using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Actor
{
    private readonly int AnimKey_Attack = Animator.StringToHash("Attack");
    
    [SerializeField] private Player target;

    [SerializeField] private bool _hasHpItem;

    [SerializeField] private float _detectRange;
    [SerializeField] private float _attackRange;


    [SerializeField] private Image _hpGuage;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gravity;

    [SerializeField] private float _knockBackJumpPower;
    [SerializeField] private float _knockBackPushPower;
    [SerializeField] private float _knockBackCoolTime;

    [SerializeField] private int _attackPower;
    [SerializeField] private float _attackCooltime;

    private CharacterController _controller;
    private Animator _animator;

    private Vector3 _moveDir = Vector3.zero;

    private bool _isKnockBack = false;
    private Vector3 _knockBackForce;
    private float _lastKnockBackTime;

    private float _lastAttackTime;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        
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

                var mag = dir.magnitude;
                if (mag < _detectRange)
                {
                    _moveDir = dir.normalized * _moveSpeed;
                    
                    if (mag < _attackRange)
                    {
                        Attack();
                    }
                    else
                    {
                        _lastAttackTime = Time.time;
                    }
                }

                transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
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
        if (_lastAttackTime + _attackCooltime < Time.time)
        {
            _animator.SetTrigger(AnimKey_Attack);
            target.Damage(transform.position, _attackPower);
            
            _lastAttackTime = Time.time;
        }
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