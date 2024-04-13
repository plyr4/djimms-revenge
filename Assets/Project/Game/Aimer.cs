using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    [SerializeField]
    private Transform _launchPoint;
    [SerializeField]
    private float _rotationSpeed = 5f;
    [SerializeField]
    [ReadOnlyInspector]
    private Vector3 _mouseWorldPosition;
    [SerializeField]
    [ReadOnlyInspector]
    private Vector3 _lookPosition;

    [SerializeField]
    [ReadOnlyInspector]
    private Vector3 _desiredDirection;
    [SerializeField]
    [ReadOnlyInspector]
    private Vector3 _desiredDirectionNormalized;
    [SerializeField]
    [ReadOnlyInspector]
    private float _desiredDirectionMagnitude;
    [SerializeField]
    [ReadOnlyInspector]
    private float _desiredAngle;
    [SerializeField]
    [ReadOnlyInspector]
    private float _desiredAngleDegrees;
    [SerializeField]
    [ReadOnlyInspector]
    [Space(5)]
    private float _debugValue;

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Update()
    {
        _lookPosition = Project.Input.GameInput.Instance._lookPosition;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_lookPosition);
        _mouseWorldPosition.z = 0f;
    }

    private void Rotate()
    {
        Vector3 direction = _mouseWorldPosition - transform.position;

        _desiredDirection = direction;
        _desiredDirectionNormalized = direction.normalized;
        _desiredDirectionMagnitude = direction.magnitude;

        // Check if the direction vector's magnitude is greater than a threshold
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x);
            _desiredAngle = angle;
            float angleDegrees = Mathf.Rad2Deg * angle - 90f;
            _desiredAngleDegrees = angleDegrees;
            Quaternion desiredRotation = Quaternion.Euler(0, 0, angleDegrees);

            _debugValue = Mathf.Abs(desiredRotation.eulerAngles.z - transform.rotation.eulerAngles.z);
            if (Mathf.Abs(desiredRotation.eulerAngles.z - transform.rotation.eulerAngles.z) > 0.1f)
            {
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }

    public Vector3 LaunchPosition()
    {
        return _launchPoint.position;
    }

    public Vector3 LaunchDirection()
    {
        return _launchPoint.up;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_launchPoint.position, 0.05f);
        Debug.DrawLine(_launchPoint.position, _launchPoint.position + LaunchDirection() * 2f, Color.red);
        Debug.DrawLine(transform.position, Vector3.zero, Color.magenta);
        Debug.DrawLine(transform.position, _lookPosition, Color.cyan);
        Debug.DrawLine(transform.position, _mouseWorldPosition, Color.yellow);
    }
}