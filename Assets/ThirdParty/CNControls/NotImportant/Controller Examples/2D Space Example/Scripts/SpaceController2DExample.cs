using System;
using UnityEngine;
using System.Collections;

public class SpaceController2DExample : MonoBehaviour
{
    const float RotateSpeed = 15f;

    public CNAbstractController MovementJoystick;
    public CNButton FireButton;

    public float Speed = 5f;

    public Laser LaserPrefab;

    private Transform _shootFromPoint;
    private Transform _transformCache;

    void Start()
    {
        FireButton.FingerTouchedEvent += FireLaser;
        MovementJoystick.ControllerMovedEvent += MoveSpaceship;

        _transformCache = GetComponent<Transform>();
        _shootFromPoint = _transformCache.GetChild(0);
    }

    private void MoveSpaceship(Vector3 movement, CNAbstractController cnAbstractController)
    {
        StopCoroutine("RotateCoroutine");
        StartCoroutine("RotateCoroutine", movement);

        _transformCache.position += _transformCache.up * movement.magnitude * Speed * Time.deltaTime;
    }

    void Update()
    {
        if (FireButton.GetButtonUp(""))
        {
            //Instantiate(LaserPrefab.gameObject, _shootFromPoint.position, _transformCache.rotation);
        }
    }

    IEnumerator RotateCoroutine(Vector3 direction)
    {
        do
        {
            _transformCache.up = Vector3.Lerp(_transformCache.up, direction, RotateSpeed * Time.deltaTime);
            yield return null;
        }
        while ((direction - _transformCache.up).sqrMagnitude > 0.2f);
    }

    private void FireLaser(CNAbstractController obj)
    {
        Instantiate(LaserPrefab.gameObject, _shootFromPoint.position, _transformCache.rotation);
    }


}
