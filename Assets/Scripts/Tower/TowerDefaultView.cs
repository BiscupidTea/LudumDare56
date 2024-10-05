using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefaultView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TowerDefault _default;

    [SerializeField] private string _attackParameter = "isAttacking";
    [SerializeField] private float _timeAnimation = 0.5f;

    private void OnEnable()
    {
        _default.OnShoot += HandleAttack;
    }

    private void OnDisable()
    {
        _default.OnShoot -= HandleAttack;
    }

    private void HandleAttack()
    {
        _animator.SetBool(_attackParameter,true);

    }

    private IEnumerator WaitingForFalseAttack()
    {
        yield return new WaitForSeconds(_timeAnimation);
        _animator.SetBool(_attackParameter, false);
    }
}
