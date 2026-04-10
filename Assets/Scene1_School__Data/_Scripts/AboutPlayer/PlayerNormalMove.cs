using UnityEngine;

public class PlayerNormalMove : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // --- ส่วนที่เพิ่ม: ระบบแรงโน้มถ่วง ---
    public float gravity = -9.81f; // ค่าแรงโน้มถ่วงมาตรฐาน
    Vector3 velocity; // เก็บความเร็วปัจจุบัน (รวมแรงดิ่ง)
    // ---------------------------------

    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {   
        if (!controller.enabled) return;
        // 1. จัดการเรื่องแรงโน้มถ่วงก่อน
        if (controller.isGrounded && velocity.y < 0)
        {
            // ถ้าเท้าแตะพื้นแล้ว ให้ค่าแรงดิ่งเป็นค่าติดลบนิดๆ เพื่อให้ตัวละครแนบติดพื้นไว้
            velocity.y = -2f; 
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (anim != null)
        {
            anim.SetFloat("Speed", direction.magnitude);
        }

        // 2. การเคลื่อนที่แนวราบ (X, Z)
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // 3. คำนวณและสั่งให้ตกตามแรงโน้มถ่วง (แกน Y)
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime);
    }
}