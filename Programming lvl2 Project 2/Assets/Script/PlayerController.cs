using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base Vars")]
    public float speed = 250f;
    public float lookSpeed = 50f;

    [Header("Jump Vars")]
    public float jumpForce = 300f;
    public bool canJump;
    public bool jumped;

    [Header("Kick Vars")]
    public Transform myFoot;
    public float kickForce = 800f;
    public float upForce = 100f;
    public float legLength = 5f;

    Rigidbody myRB;
    public Camera myCam;
    public float camLock;

    Vector3 myLook;
    float onStartTimer;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Quaternion currentRot = transform.rotation;
        myCam.transform.rotation = currentRot;
        myLook = myCam.transform.forward;
    }
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();       
        canJump = true;
        jumped = false;
        onStartTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        onStartTimer += Time.deltaTime;
        Vector3 playerLook = myCam.transform.forward;
        
        Debug.DrawRay(transform.position, playerLook * 3f, Color.green);
        myLook += DeltaLook() * Time.deltaTime;
        myLook.y = Mathf.Clamp(myLook.y, -camLock, camLock);

       // myLook = Vector3.ClampMagnitude(myLook, camLock);

        transform.rotation = Quaternion.Euler(0f,myLook.x, 0f);
        myCam.transform.rotation = Quaternion.Euler(-myLook.y, myLook.x, 0f);

        if(Input.GetKey(KeyCode.Space) && canJump)
        {
            jumped = true;
        }
        else
        {
            jumped = false;
        }
        if (Input.GetKey(KeyCode.Return))
        {
            Kick();
        }
    }
    void FixedUpdate()
    {
        Vector3 pMove = transform.TransformDirection(Dir());
        myRB.AddForce(pMove * speed * Time.fixedDeltaTime);

        //Debug.DrawRay(transform.position, pMove * 5f, Color.magenta);
        //Debug.DrawRay(transform.position, Vector3.up, Color.magenta);

       // Debug.DrawRay(transform.position + Vector3.up, myRB.velocity.normalized*5f, Color.black);

        if(jumped && canJump)
        {
            Jump();
        }
    }
    Vector3 Dir()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 myDir = new Vector3(x, 0, z);

        if (myDir != Vector3.zero)
        {
            Debug.Log("Player Move Dir: " + myDir);
        }
        return myDir;
    }
    Vector3 DeltaLook()
    {
        Vector3 dLook = Vector3.zero;
        float rotY = Input.GetAxisRaw("Mouse Y") * lookSpeed;
        float rotX = Input.GetAxisRaw("Mouse X") * lookSpeed;
        dLook = new Vector3(rotX, rotY, 0);
        
        if(dLook != Vector3.zero)
        {
            //Debug.Log("delta look: " + dLook);
        }

        if(onStartTimer < 1f)
        {
            dLook = Vector3.ClampMagnitude(dLook, onStartTimer * 10f);
        }

        return dLook;

    }
    void Jump()
    {
        myRB.AddForce(Vector3.up * jumpForce);
        jumped = false;
    }
    void Kick()
    {
        RaycastHit hit;
        bool rayCast = false;
        //bool rayCast = Physics.Raycast(transform.position, Vector3.forward, 5f);

        Vector3 kickDir = new Vector3(myCam.transform.forward.x, 0f, myCam.transform.forward.z);
        if (Physics.SphereCast(myFoot.position, .2f, kickDir, out hit, legLength)) { rayCast = true; }
        Debug.Log("raycast: " + hit);
        Debug.DrawRay(myFoot.position, kickDir*legLength, Color.blue);

        if (rayCast && hit.collider.gameObject.tag == "Ball")
        {
            Debug.Log("hit BALL");
            hit.rigidbody.AddExplosionForce(kickForce, hit.point, legLength, upForce);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain") { canJump = true; }
        if (collision.gameObject.tag == "platforms")
        { StartCoroutine(killPlatform(1f, collision)); }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain") { canJump = false; }
    }
    IEnumerator killPlatform(float time, Collision collision)
    {
        yield return new WaitForSeconds(time);
        collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
