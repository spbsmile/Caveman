using UnityEngine;
using System.Collections;

public class CameraFollowExample : MonoBehaviour
{
    public CNAbstractController RotateJoystick;
    public float RotationSpeed = 10f;

    private Transform _transformCache;
    private Transform _parentTransformCache;

    // Use this for initialization
    void Start()
    {
        _transformCache = GetComponent<Transform>();
        _parentTransformCache = _transformCache.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (RotateJoystick != null)
        {
            float rotationX = RotateJoystick.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;
            float rotationY = RotateJoystick.GetAxis("Vertical") * RotationSpeed * Time.deltaTime;
            _parentTransformCache.Rotate(0f, rotationX, 0f, Space.World);
            _parentTransformCache.Rotate(-rotationY, 0f, 0f);
        }

        /*
        if (Target != null)
        {
            if (RotateJoystick != null)
            {
                float rotation = RotateJoystick.GetAxis("Horizontal");
                _transformCache.RotateAround(Target.position, Vector3.up, rotation * RotationSpeed * Time.deltaTime);
            }
            _transformDifference = _transformCache.position - Target.position;

            _transformCache.position = Target.position + _transformDifference.normalized * _transformDistance;
        }
        */
    }
}
