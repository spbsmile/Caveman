using System;
using UnityEngine;
using System.Collections;

public class WorldMapCamera : MonoBehaviour
{

    public float Speed = 15f;
    public CNAbstractController Touchpad;

    private Transform _transformCache;
    private Camera _camera;

    // Use this for initialization
    void Start()
    {
        Touchpad.ControllerMovedEvent += TouchpadOnControllerMovedEvent;

        _transformCache = GetComponent<Transform>();
        _camera = GetComponent<Camera>();
    }

    private void TouchpadOnControllerMovedEvent(Vector3 movement, CNAbstractController cnAbstractController)
    {
        movement = _transformCache.TransformDirection(movement);
        movement.y = 0f;

        // Здесь вместо - можно поставить +, тогда двигаться будет в сторону перемещения пальца
        _transformCache.position -= movement * Speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
