using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour {
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 0.1F;
    public float sensitivityY = 0.1F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    public int _speed = 5;
    public static int myroom = 0;
    GameObject obj_physics;
    Rigidbody rb_phy;
    Vector3 totalgrav=Vector3.zero;
    float rotationY = 0F;
    float notgrounded = 0.0f;
    bool jumped = false;
    float distToGround;
    public bool isgrounded;
    void rotation()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
    Vector3 move()
    {
        //키
        float _verticalPos = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        float _horizonPos = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        Vector3 positionup = (transform.forward * _verticalPos + transform.right * _horizonPos - new Vector3(0, transform.forward.y * _verticalPos + transform.right.y * _horizonPos, 0));
        Vector3 realmoving = Vector3.Distance(Vector3.zero, transform.forward * _verticalPos + transform.right * _horizonPos) * positionup.normalized;
        return realmoving;
    }
    Vector3 jump()
    {
        Vector3 gravity = Vector3.zero;
        Vector3 jumping = Vector3.zero;
        if (isgrounded)
        {
            notgrounded = Time.time;
            if (Input.GetKey(KeyCode.Space))
                jumped = true;
            else
                jumped = false;
        }
        else
        {
            //gravity = (Time.time - notgrounded) * Vector3.down * 9.8f *4* Time.deltaTime;
        }
        if (jumped)
        {
            jumping =Vector3.up *5;
        }
        //Debug.Log(gravity);
        return (gravity + jumping * Time.deltaTime);
        
        
    }
    void Update()
    {
        rotation();
        grounded();
        //rb_phy.MovePosition(obj_physics.transform.position + move());
        //중력
    }
    void FixedUpdate()
    {
        rb_phy.MovePosition(obj_physics.transform.position + move()+jump());
    }
    void grounded()
    {
        isgrounded = Physics.Raycast(obj_physics.transform.position, -Vector3.up, distToGround + 0.1f);
        //room 확인
        RaycastHit[] hitdahit=Physics.RaycastAll(new Ray(obj_physics.transform.position, -Vector3.up), 10f);
        for(int i=0;i<hitdahit.Length;i++)
        {
            string goname = hitdahit[i].transform.name;
            if(goname.Length>=5 && goname.Substring(0,5)=="Floor")
            {
                myroom = System.Convert.ToInt32(goname.Substring(6, goname.Length - 6));
                break;
            }
        }
    }
    void Start()
    {
        
        obj_physics = GameObject.Find("physics");
        distToGround = obj_physics.GetComponent<CapsuleCollider>().bounds.extents.y;
        rb_phy = obj_physics.GetComponent<Rigidbody>();
        // Make the rigid body not change rotation
        //if (rigidbody)
        //rigidbody.freezeRotation = true;
    }
}
