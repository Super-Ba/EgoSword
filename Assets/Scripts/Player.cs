using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    private readonly int AnimKey_Roll = Animator.StringToHash("Roll");
    private readonly int AnimKey_Attack = Animator.StringToHash("Attack");
    private readonly int AnimKey_AtkPhase = Animator.StringToHash("AtkPhase");

    [SerializeField] private GameObject _playerModel;
    [SerializeField] private GameObject _arrowObject;
    
    [SerializeField] private LayerMask _ground;
    [SerializeField] private LayerMask _enemy;

    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;

    [Header("대쉬")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashCooltime;

    [Header("공격")]
    [SerializeField] private int _totalAtkCount;//연타 애니메이션 개수
    [SerializeField] private float _atkComboMaxTime;
    [SerializeField] private float _atkAngle;
    
    
    private Sword _sword;

    private CharacterController _controller;
    private Animator _animator;
    private Transform _cameraTransform;

    private bool _isDash = false;
    private float _lastDashTime = 0;
    private Vector3 _moveDir;

    private Vector2 _aimDir;

    private float _lastAtkTime;
    private int _combo;


    private float _atkCoolTime;
    private int _atkPower;
    private float _atkRange;

    void Start()
    {
        _animator = _playerModel.GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;

        _sword = new Sword();

        _atkCoolTime = _sword.AtkCoolTime;
        _atkPower = _sword.AtkPower;
        _atkRange = _sword.AtkRange;
    }

    void Update()
    {
        Move();
        Aim();
        Attack();
        
        UpgradeSword();

        UpdateUI();
    }


    private void Move()
    {

        if (_isDash && _lastDashTime + _dashTime < Time.time)
        {
            _isDash = false;
        }

        if (_controller.isGrounded && !_isDash)
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


            var foward = _cameraTransform.forward;
            var right = _cameraTransform.right;
            foward.y = 0;
            right.y = 0;
            foward.Normalize();
            right.Normalize();

            var fowardReVerticalInput = input.y * foward;
            var rightRelHorizontalInput = input.x * right;

            _moveDir = fowardReVerticalInput + rightRelHorizontalInput;

            if (Input.GetButton("Jump") && _lastDashTime + _dashCooltime < Time.time)
            {
                _isDash = true;
                _lastDashTime = Time.time;

                _moveDir.Normalize();
                _moveDir *= _dashSpeed;

                _animator.SetTrigger(AnimKey_Roll);

                transform.rotation = Quaternion.LookRotation(_moveDir, Vector3.up);
            }
            else
            {
                _moveDir *= _speed;
            }
        }

        _moveDir.y -= _gravity;

        _controller.Move(_moveDir * Time.deltaTime);
    }

    private void Aim()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!_isDash && Physics.Raycast(ray, out var hit, Mathf.Infinity, _ground))
        {
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);

            var dir = (hit.point - transform.position).normalized;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

            _aimDir.x = dir.x;
            _aimDir.y = dir.y;
        }
    }

    protected override void Attack()
    {
        if (_isDash)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && _lastAtkTime + _atkCoolTime < Time.time)
        {
            if (_lastAtkTime + _atkComboMaxTime > Time.time)
            {
                _combo++;
                if (_combo > _totalAtkCount)
                {
                    _combo = 0;
                }
            }

            _animator.SetInteger(AnimKey_AtkPhase, _combo);
            _animator.SetTrigger(AnimKey_Attack);


            var result = Physics.OverlapSphere(transform.position, _atkRange, _enemy);

            foreach (var hit in result)
            {
                var dir = hit.transform.position - transform.position;
                var dot = Vector3.Dot(transform.forward, dir.normalized);
                var angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angle < _atkAngle && hit.TryGetComponent<Actor>(out var actor))
                {
                    actor.Damage(transform.position, _atkPower);
                }
            }


            _lastAtkTime = Time.time;
        }

    }

    private void UpdateUI()
    {
        UIManager.Instance.SetDashGuage((Time.time - _lastDashTime) / _dashCooltime);
        UIManager.Instance.SetHpGuage(Hp, MaxHp);
        UIManager.Instance.SetLevelGuage(_sword.Level);
        UIManager.Instance.SetCostGuage(_sword.NextCost);
        
        _arrowObject.SetActive(!_isDash);
    }

    private void UpgradeSword()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_sword.LevelUp())
            {
                Hp -= _sword.NextCost;

                _atkCoolTime = _sword.AtkCoolTime;
                _atkPower = _sword.AtkPower;
                _atkRange = _sword.AtkRange;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * (_dashTime * _dashSpeed));

        var angleMin = (_atkAngle + transform.rotation.eulerAngles.y) * Mathf.Deg2Rad;
        var angleMax = (-_atkAngle + transform.rotation.eulerAngles.y) * Mathf.Deg2Rad;

        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(Mathf.Sin(angleMin), 0, Mathf.Cos(angleMin)) * _atkRange);
        Gizmos.DrawRay(transform.position, new Vector3(Mathf.Sin(angleMax), 0, Mathf.Cos(angleMax)) * _atkRange);
    }

}