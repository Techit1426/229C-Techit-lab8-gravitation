using UnityEngine;
using System.Collections.Generic;

public class Gravitation : MonoBehaviour
{
    private Rigidbody rb;
    private const float G = 0.006674f;

    // เก็บวัตถุทั้งหมดที่มีสคริปต์นี้
    public static List<Gravitation> otherObjectList;

    [Header("Optional Orbit Setting")]
    public bool isCenterObject = false;   // เช่น Earth
    public float orbitSpeed = 0f;         // ใช้กับดาวที่ต้องการให้เริ่มโคจร

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (otherObjectList == null)
            otherObjectList = new List<Gravitation>();

        otherObjectList.Add(this);
    }

    void Start()
    {
        // ใส่แรงเริ่มต้นเพื่อให้เกิดการโคจร
        if (!isCenterObject && orbitSpeed > 0f)
        {
            rb.AddForce(Vector3.left * orbitSpeed, ForceMode.Force);
        }
    }

    void FixedUpdate()
    {
        foreach (Gravitation obj in otherObjectList)
        {
            // ป้องกันไม่ให้วัตถุดึงตัวเอง
            if (obj != this)
            {
                AttractForce(obj);
            }
        }
    }

    void AttractForce(Gravitation other)
    {
        Rigidbody otherRb = other.rb;

        // หาทิศทางระหว่างวัตถุ
        Vector3 direction = rb.position - otherRb.position;

        // หาระยะห่างระหว่างวัตถุ
        float distance = direction.magnitude;

        // ถ้าใกล้เกินไปให้ข้าม ป้องกันแรงพุ่งสูงเกิน
        if (distance <= 0.1f)
            return;

        // สูตรแรงดึงดูด F = G * ((m1 * m2) / r^2)
        float forceMagnitude = G * ((rb.mass * otherRb.mass) / Mathf.Pow(distance, 2));

        // หาเวกเตอร์แรงดึงดูด
        Vector3 gravityForce = forceMagnitude * direction.normalized;

        // เพิ่มแรงให้วัตถุอีกตัว
        otherRb.AddForce(gravityForce, ForceMode.Force);
    }

    void OnDestroy()
    {
        if (otherObjectList != null && otherObjectList.Contains(this))
        {
            otherObjectList.Remove(this);
        }
    }
}