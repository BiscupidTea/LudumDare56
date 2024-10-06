using UnityEngine;

public class TowerDefaultView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TowerDefault _default;

    [SerializeField] private string _attackParameter = "attack";

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
        _animator.SetTrigger(_attackParameter);

    }
}
