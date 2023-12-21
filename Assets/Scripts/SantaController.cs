using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaController : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Rigidbody2D _rb2D;

    private float _speed = 5.0f;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Walk();
    }

    private void Walk()
    {
        _rb2D.velocity = _joystick.Direction * _speed;
    }
}
