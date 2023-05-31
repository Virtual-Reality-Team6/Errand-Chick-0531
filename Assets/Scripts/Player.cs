using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpUp;
    float hAxis;
    float vAxis;
    bool shiftDown;
    bool spaceDown;
    bool interactionDown;
    bool memoDown;
    bool stampDown;

    public int coin;
    public int score;

    bool isJump;
    bool isShop;
    bool seeMemo;
    bool seeStamp;

    public bool[] iscompletedErrand;
    public bool[] iscompletedStamp;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;

    public RectTransform memoUIGroup;
    public RectTransform stampUIGroup;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        //PlayerPrefs.SetInt("MaxScore", 112500);
    }
    
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Interaction();
        Memo();
        Stamp();
        
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        shiftDown = Input.GetButton("Run");
        spaceDown = Input.GetButton("Jump");
        interactionDown = Input.GetButtonDown("Interaction");
        memoDown = Input.GetButtonDown("Memo");
        stampDown = Input.GetButtonDown("Stamp");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //don't use y-axis
        //.normalized: no matter direction

        transform.position += moveVec * speed * (shiftDown ? 2f : 1f) * Time.deltaTime;

        anim.SetBool("Walk", moveVec != Vector3.zero); //if moveVec is not zero(stop), Walk is True => execute Walk Anim 
        anim.SetBool("Run", shiftDown); // 3-2. Run

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec); // 4-1. look at 
    }

    void Jump()
    {
        if (spaceDown && !isJump && !isShop){
            rigid.AddForce(Vector3.up * jumpUp,  ForceMode.Impulse);
            anim.SetBool("Jump", true);
            anim.SetTrigger("Jump");
            isJump = true;
        }
    }

    void Interaction()
    {
        if(interactionDown && nearObject != null){
            if(nearObject.tag == "NPC"){
                Shop npc = nearObject.GetComponent<Shop>();
                npc.Enter(this);
                isShop = true;
            }
        }
    }

    void Memo()
    {
        if(memoDown){
            if(seeMemo) {
                memoUIGroup.anchoredPosition = Vector3.down * 1000;
                seeMemo = false;
            }
            else {
                memoUIGroup.anchoredPosition = Vector3.zero;
                seeMemo = true;
            }
        }
    }

    void Stamp()
    {
        if(stampDown){
            if(seeStamp) {
                stampUIGroup.anchoredPosition = Vector3.down * 1000;
                seeStamp = false;
            }
            else {
                stampUIGroup.anchoredPosition = new Vector3(-300f, 0f, 0f);
                seeStamp = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor"){
            isJump = false;
        }
    }

    void OnTriggerStay(Collider other){
        if(other.tag == "NPC")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "NPC"){
            Shop npc = nearObject.GetComponent<Shop>();
            npc.Exit();
            isShop = false;
            nearObject = null;
        }
    }


}
