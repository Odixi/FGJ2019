﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera camera;
    private Rigidbody rigidbody;
    private CharacterController cc;
    [SerializeField]
    private float cameraTargetDist;
    [SerializeField]
    private float cameraTargetDistErr = 0.5f;
    [SerializeField]
    private float cameraBackwarsMoveStep = 0.05f;
    [SerializeField]
    private float cameraTragetHeight;
    [SerializeField]
    private float cameraSens;
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationInterpolationMultipler = 0.1f;
    [SerializeField]
    private float cameraInterpolationMultipler = 0.1f;

    private Vector3 moveVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var ms = Mathf.Clamp(Mathf.Abs(hor) + Mathf.Abs(ver), 0, moveSpeed);
        moveVec.y -= gravity * Time.fixedDeltaTime;
        if (hor != 0 || ver != 0)
        {
            var dir = camera.transform.TransformDirection(new Vector3(hor, 0, ver));
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.1f);
            moveVec = transform.TransformDirection(new Vector3(0, moveVec.y, ms));
        }
        if (cc.isGrounded)
        {
            moveVec.y = 0;
            if (Input.GetButton("Jump"))
            {
                moveVec.y = jumpSpeed;
            }
        }
        cc.Move(moveVec);

    }
    // Update is called once per frame
    void Update()
    {
        var mh = Input.GetAxis("Mouse X");
        var mv = -Input.GetAxis("Mouse Y");

        camera.transform.RotateAround(transform.position, Vector3.up, mh * cameraSens);
        camera.transform.RotateAround(transform.position, camera.transform.right, mv * cameraSens);

        if (Vector3.SqrMagnitude(transform.position - camera.transform.position) > cameraTargetDist + cameraTargetDistErr)
        {
            camera.transform.position = Vector3.Slerp(camera.transform.position, transform.position, cameraInterpolationMultipler);
        }
        else if (Vector3.SqrMagnitude(transform.position - camera.transform.position) < cameraTargetDist - cameraTargetDistErr)
        {
            camera.transform.position -= (transform.position - camera.transform.position).normalized * cameraBackwarsMoveStep;
        }

        camera.transform.position = new Vector3(camera.transform.position.x, Mathf.Lerp(camera.transform.position.y, transform.position.y + cameraTragetHeight, cameraInterpolationMultipler) ,camera.transform.position.z);

        camera.transform.LookAt(transform);



    }
}