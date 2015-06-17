using UnityEngine;

public class Laser : MonoBehaviour
{
    public float Speed = 15f;

    private Transform _transformCache;

    void Start()
    {
        _transformCache = GetComponent<Transform>();

        Destroy(gameObject, 3f);
    }

    void Update()
    {
        _transformCache.position += _transformCache.up * Speed * Time.deltaTime;
    }

    
}
