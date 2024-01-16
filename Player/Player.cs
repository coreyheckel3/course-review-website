using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        Vector3 velocity = moveDirection * speed;

        controller.Move(velocity * Time.deltaTime);
    }
}