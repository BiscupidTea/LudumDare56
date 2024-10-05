using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemySo", menuName = "Create New Enemy")]
public class BaseEnemySO : ScriptableObject
{
    public float speed;
    public float maxLife;
    public float damage;
}
