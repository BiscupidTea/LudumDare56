using UnityEngine;

[CreateAssetMenu(fileName = "BaseTowerSo", menuName = "Create New Tower")]
public class BaseTowerSO : ScriptableObject
{
    public int damage;
    public float fireRate;
    public float attackRadius;
    public float rotationSpeed;
    public float shootAnimationDuration;
    public float bulletSpeed;
    public float price;
}

