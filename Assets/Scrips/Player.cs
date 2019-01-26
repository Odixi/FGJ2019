using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera camera;
    private Rigidbody rigidbody;
    private CapsuleCollider collider;
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
    private float jumpSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float moveAcc;
    [SerializeField]
    private float rotationInterpolationMultipler = 0.1f;
    [SerializeField]
    private float cameraInterpolationMultipler = 0.1f;
    [SerializeField]
    private float cameraHeightInterpolationMultipler = 0.1f;
    private GameObject mount;
    private Vector3? mountLastPosition;

    Animator animator;

    private Vector3 moveVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        var ms = Mathf.Clamp(Mathf.Abs(hor) + Mathf.Abs(ver), 0, 1);
        moveVec.x = 0;
        moveVec.z = 0;
        if (hor != 0 || ver != 0)
        {
            var dir = camera.transform.TransformDirection(new Vector3(hor, 0, ver));
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationInterpolationMultipler);
            
        }
        RaycastHit hit;
        if (Physics.SphereCast(new Ray(transform.TransformPoint(collider.center), Vector3.down), collider.radius * transform.localScale.x, out hit, 1.5f * transform.localScale.y))
        {
            if (hit.collider.CompareTag("Rideable"))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject != mount)
                {
                    mount = hit.collider.gameObject;
                    mountLastPosition = null;
                }
            }
            else
            {
                mount = null;
            }
            animator.SetBool("Jumping", false);
            moveVec.y = 0;
            if (Input.GetButton("Jump"))
            {
                animator.SetBool("Jumping", true);
                rigidbody.AddForce(Vector3.up * jumpSpeed);
            }
        }
        else
        {
            mount = null;
        }
        if (mount != null)
        {
            Ride();
        }
        print(rigidbody.velocity.sqrMagnitude);
        animator.SetFloat("Speed", 1.7f*rigidbody.velocity.sqrMagnitude / moveSpeed);
        var zm = Mathf.Max( moveAcc*(moveSpeed - rigidbody.velocity.sqrMagnitude), 0);
        moveVec = transform.TransformDirection(new Vector3(0, 0, ms*zm));
        rigidbody.AddForce(moveVec);

    }

    void Ride()
    {
        Vector3 newPos = mount.transform.position;
        newPos.y = transform.position.y;
        if (mountLastPosition != null)
        {
            print("mount moved to " + (mount.transform.position - (Vector3)mountLastPosition));
            transform.position += mount.transform.position - (Vector3)mountLastPosition;
        }
        mountLastPosition = mount.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var mh = Input.GetAxis("Mouse X");
        var mv = -Input.GetAxis("Mouse Y");

        camera.transform.RotateAround(transform.position, Vector3.up, mh * cameraSens);
        camera.transform.RotateAround(transform.position, camera.transform.right, mv * cameraSens);
        var sqrDist = Vector3.SqrMagnitude(transform.position - camera.transform.position);
        if (sqrDist > cameraTargetDist + cameraTargetDistErr)
        {
            camera.transform.position = Vector3.Slerp(camera.transform.position, transform.position, cameraInterpolationMultipler * sqrDist);
        }
        else if (sqrDist < cameraTargetDist - cameraTargetDistErr)
        {
            camera.transform.position -= (transform.position - camera.transform.position).normalized * cameraBackwarsMoveStep;
        }

        //camera.transform.position = new Vector3(camera.transform.position.x, Mathf.Lerp(camera.transform.position.y, transform.position.y + cameraTragetHeight, cameraHeightInterpolationMultipler) ,camera.transform.position.z);

        camera.transform.LookAt(transform.position + Vector3.up * 0.1f);

    }


    //private GameObject lastCollided;
    //private Vector3 lastCollidedOldPosition;
    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.gameObject == null)
    //    {
    //        return;
    //    }

    //    //if ((cc.collisionFlags & CollisionFlags.Below) != 0)
    //    //{
    //        if (lastCollided == hit.gameObject)
    //        {
    //            transform.position += (hit.gameObject.transform.position - lastCollidedOldPosition);
    //        }
    //        else
    //        {
    //            lastCollided = hit.gameObject;
    //        }
    //        lastCollidedOldPosition = lastCollided.transform.position;
    //    //}
    //}



}
