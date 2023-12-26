using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    private Animator _animator;
    private Coroutine _returnIdleCoroutine;
    private const int MAX_HEALTH = 100;
    private int _currentHealth;
    [SerializeField] private HealthBar _healthBar;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = MAX_HEALTH;
        _healthBar.SetMaxHealth(MAX_HEALTH);
    }

    public void Hurt()
    {
        if(_returnIdleCoroutine != null) StopCoroutine(_returnIdleCoroutine);
        _animator.Play("Skeleton_Hurt");
        _currentHealth -= 15;
        _healthBar.SetHealth(_currentHealth);
        _returnIdleCoroutine = StartCoroutine(ReturnIdleState());

        if (_currentHealth <= 0) Destroy(gameObject);
    }

    private IEnumerator ReturnIdleState()
    {
        float timeForTransition = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(timeForTransition);
        _animator.Play("Skeleton_Idle");
    }
    
}
