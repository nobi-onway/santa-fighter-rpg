using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SantaController : MonoBehaviour
{
    private enum SantaState
    {
        idle,
        walk,
        punch_1,
        punch_2,
        kick_1,
        kick_2,
        smash_down,
    }

    [SerializeField] private Joystick _joystick;
    [SerializeField] private Button _attackBtn;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _attackPoint;

    private Coroutine _returnIdleStateCoroutine;
    private int _comboAttack;
    private SantaState[] _attackStates = {SantaState.punch_1, SantaState.punch_2, SantaState.kick_1, SantaState.kick_2, SantaState.smash_down};

    private SantaState CurrentState 
    { 
        get => _currentState; 
        set 
        {
            if(_returnIdleStateCoroutine != null) StopCoroutine(_returnIdleStateCoroutine);
            _currentState = value;
            PlayAnimation(value);
        } 
    }

    [SerializeField]
    private SantaState _currentState;

    private float _speed = 5.0f;


    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _attackBtn.onClick.AddListener(Attack);
        _comboAttack = 0;
    }

    private void Update()
    {
        Walk();
    }

    private void Walk()
    {
        _rb2D.velocity = _joystick.Direction * _speed;
        if (_joystick.Direction != Vector2.zero) CurrentState = SantaState.walk;
    }

    private void Attack()
    {
        if(_comboAttack == 0)
        {
            CurrentState = SantaState.punch_1;
            Collider2D hitSkeleton = Physics2D.OverlapCircle(_attackPoint.position, 0.15f);

            if (hitSkeleton != null)
                hitSkeleton.GetComponent<SkeletonController>()?.Hurt();

            _comboAttack++;
            return;
        }

        StartCoroutine(ComboAttack(_attackStates[_comboAttack]));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CurrentState == SantaState.idle || CurrentState == SantaState.walk) return;

        SkeletonController skeletonController = collision.gameObject.GetComponent<SkeletonController>();
        if (!skeletonController) return;

        skeletonController.Hurt();
    }

    private IEnumerator ComboAttack(SantaState state)
    {
        float timeForTransition = _animator.GetCurrentAnimatorStateInfo(0).length - 0.25f;
        yield return new WaitForSeconds(timeForTransition);
        Collider2D hitSkeleton = Physics2D.OverlapCircle(_attackPoint.position, 0.15f);

        if (hitSkeleton != null)
            hitSkeleton.GetComponent<SkeletonController>()?.Hurt();

        CurrentState = state;

        _comboAttack = _comboAttack < _attackStates.Length - 1 ? _comboAttack + 1 : 0;
    }


    private IEnumerator ReturnIdleState()
    {
        float timeForTransition = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(timeForTransition);
        CurrentState = SantaState.idle;
        _comboAttack = 0;
    }

    private void FlipX()
    {
        if (_joystick.Direction == Vector2.zero) return;
        transform.localScale = _joystick.Direction.x < 0 ? new Vector2(-1, 1) : new Vector2(1, 1);
    }

    private void PlayAnimation(SantaState state)
    {
        switch (state)
        {
            case SantaState.idle:
                _animator.Play("Santa_Idle");
                break;
            case SantaState.walk:
                FlipX();
                _animator.Play("Santa_Walk");
                break;
            case SantaState.punch_1:
                _animator.Play("Santa_Punch_1");
                break;
            case SantaState.punch_2:
                _animator.Play("Santa_Punch_2");
                break;
            case SantaState.kick_1:
                _animator.Play("Santa_Kick_1");
                break;
            case SantaState.kick_2:
                _animator.Play("Santa_Kick_2");
                break;
            case SantaState.smash_down:
                _animator.Play("Santa_Smash_Down");
                break;
        }

        _returnIdleStateCoroutine = StartCoroutine(ReturnIdleState());
    }
}
