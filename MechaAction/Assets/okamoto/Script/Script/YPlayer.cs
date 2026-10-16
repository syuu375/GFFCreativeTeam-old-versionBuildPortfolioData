using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ���E�ړ��i�����̂݁j
        float moveX = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(moveX * moveSpeed, velocity.y, velocity.z);

        // �W�����v
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // �n�ʂɐڂ��Ă���Ƃ������W�����v�\�ɂ���
        if (collision.gameObject.CompareTag("Grounded"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            isGrounded = false;
        }
    }
}
