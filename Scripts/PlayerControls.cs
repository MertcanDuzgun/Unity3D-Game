using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public GameManager manage;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private InputActionAsset asset;

    // Oyuncunun geliþtirmelerden baðýmsýz orijinal hýzý.
    public float playerSpeedO = 6.0f;

    // Oyuncunun geliþtirmelere dayalý olarak deðiþeceði hýzý.
    public float playerSpeed;

    // Paralarýn oluþacaðý y düzlemindeki konumun ayarlanmasý için alan.
    public float yAxis = 0;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

       
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }


        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}