using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float rotationSpeed = 30f; // Dönme hýzý (derece/s)
    public float radius = 5f; // Dairesel hareketin yarýçapý

    private float angle = 0f; // Baþlangýç açýsý

    private void Update()
    {
        // Zamanla sürekli dairesel hareket için dönme açýsýný güncelleme
        angle += rotationSpeed * Time.deltaTime;

        // Yeni konumu hesapla ve nesneyi hareket ettir
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
