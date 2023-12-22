using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    private Animator _animator;
    private Coroutine _returnIdleCoroutine;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hurt()
    {
        if(_returnIdleCoroutine != null) StopCoroutine(_returnIdleCoroutine);
        _animator.Play("Skeleton_Hurt");
        _returnIdleCoroutine = StartCoroutine(ReturnIdleState());
    }

    private IEnumerator ReturnIdleState()
    {
        float timeForTransition = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(timeForTransition);
        _animator.Play("Skeleton_Idle");
    }
    
}
