using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ovni : MonoBehaviour
{
    [SerializeField] private List<Transform> _toPosition;
    [SerializeField] private float _speed = 1;
    private float distanceToReachPoint = 0.4f;
    private int _currentIndex = 0;

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _toPosition[_currentIndex + 1].position, _speed * Time.deltaTime);
        RotateTowardsTarget();

        if (Vector2.Distance(transform.position, _toPosition[_currentIndex + 1].position) <= distanceToReachPoint)
        {
            _currentIndex++;

            if (_currentIndex >= _toPosition.Count - 1)
            {
                _currentIndex = 0;
            }
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(_toPosition[_currentIndex + 1].position.y - transform.position.y, _toPosition[_currentIndex + 1].position.x - transform.position.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 0 * Time.deltaTime);
    }
}
