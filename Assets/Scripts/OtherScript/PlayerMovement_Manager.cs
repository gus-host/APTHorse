using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Manager : MonoBehaviour
{
    [SerializeField] AnimationSwitch animSwitch;

    public float moveSpeed = 4.0f;      
    public float rotateSpeed = 1.5f;

    public bool jumpAnimation = false;
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isDead = false;

   
    public float attackTimer = 0f;
    public float attackDuration = 0.7f;

    Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxis("Vertical");

        


        // Rotate around y-axis
        transform.Rotate(0, Input.GetAxis("Horizontal")*rotateSpeed, 0);


        if (Input.GetMouseButtonDown(0) && !isDead && !jumpAnimation)
        {
            isAttacking = true;
            animSwitch.AnimateAttack();
            attackTimer = 0;
        }
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDuration) isAttacking = false;
        }


        // Forward is the forward direction for this character
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // You need the Character Controller so you can use SimpleMove
            CharacterController controller = GetComponent<CharacterController>();
            controller.SimpleMove(forward * speed*moveSpeed);

        if (speed != 0)
        {
            if (!jumpAnimation && !isAttacking && !isDead) animSwitch.AnimateWalk();
        }
        else
        {
            if (!jumpAnimation && !isAttacking && !isDead) animSwitch.AnimateIDLE();
        }

    }

    private void FixedUpdate()
    {
    }
}
