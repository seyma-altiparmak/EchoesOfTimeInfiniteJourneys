using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float rotationSpeed = 30f; // D�nme h�z� (derece/s)
    public float radius = 5f; // Dairesel hareketin yar��ap�

    private float angle = 0f; // Ba�lang�� a��s�

    private void Update()
    {
        // Zamanla s�rekli dairesel hareket i�in d�nme a��s�n� g�ncelleme
        angle += rotationSpeed * Time.deltaTime;

        // Yeni konumu hesapla ve nesneyi hareket ettir
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
