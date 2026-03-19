using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody rb;

    // ค่าคงที่แรงโน้มถ่วง ปรับเล็ก ๆ ก่อนเพื่อให้ดูดช้า ๆ
    [SerializeField] private float G = 0.006674f;

    // Optional: ใช้กำหนดว่า object นี้เป็นศูนย์กลางหรือเป็นดาวที่โคจร
    [SerializeField] private bool planet = false;   // Earth = true, ดาวอื่น = false
    [SerializeField] private float orbitSpeed = 0f; // Earth = 0, ดาวอื่นใส่ค่าได้

    // เก็บวัตถุทั้งหมดที่มี Gravity script
    public static List<Gravity> gravityObjectsList;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (gravityObjectsList == null)
        {
            gravityObjectsList = new List<Gravity>();
        }

        gravityObjectsList.Add(this);
    }

    private void OnDestroy()
    {
        if (gravityObjectsList != null && gravityObjectsList.Contains(this))
        {
            gravityObjectsList.Remove(this);
        }
    }

    private void Start()
    {
        // Optional: เพิ่มแรงด้านข้างเพื่อให้เกิดการโคจรเบื้องต้น
        if (!planet && orbitSpeed > 0f)
        {
            rb.AddForce(Vector3.left * orbitSpeed, ForceMode.Force);
        }
    }

    private void FixedUpdate()
    {
        foreach (Gravity obj in gravityObjectsList)
        {
            if (obj != this)
            {
                Attract(obj);
            }
        }
    }

    void Attract(Gravity other)
    {
        Rigidbody otherRb = other.rb;

        Vector3 direction = rb.position - otherRb.position;
        float distance = direction.magnitude;

        // กันหารด้วย 0 หรือระยะใกล้เกินไป
        if (distance < 0.1f) return;

        // F = G * m1 * m2 / r^2
        float forceMagnitude = G * (rb.mass * otherRb.mass) / (distance * distance);

        Vector3 force = direction.normalized * forceMagnitude;

        otherRb.AddForce(force);
    }
}