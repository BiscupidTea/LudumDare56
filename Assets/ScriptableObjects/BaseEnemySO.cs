using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemySo", menuName = "Create New Enemy")]
public class BaseEnemySO : ScriptableObject
{
    public string enemyName = "Base";
    public Sprite asset;
    public float speed = 1f;
    public float maxLife = 1f;
    public float damage = 1f;
}
