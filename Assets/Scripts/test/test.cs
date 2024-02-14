using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
     float minRotationSpeed = 20f, maxRotationSpeed = 50f; 
     float minHeight = 3f, maxHeight = 7f; 
     float radius = 5f; 
     float followDuration = 15f; 
     float raycastDistance = 10f; 
     Light _light;

    [Header("Player Detection")]
    [SerializeField] LayerMask targetLayer;
    Vector3 _position;

    float angle, height; 
    float followTimer = 0f; 
    bool isFollowingPlayer = false; 


    private void Awake()
    {
        _light = GetComponentInChildren<Light>();
    }

    private void Start()
    {
        angle = Random.Range(0f, 360f);
        height = Random.Range(minHeight, maxHeight);
        _position = new Vector3(Random.Range(-40f, 5f), Random.Range(-45f, 0f), height);
        transform.position = _position;
    }

    private void Update()
    {
        angle += Random.Range(minRotationSpeed, maxRotationSpeed) * Time.deltaTime;
        height += Random.Range(-0.5f, 0.5f) * Time.deltaTime;

        //Pozisyon hesabý
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        transform.position = new Vector3(x, height, z);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, targetLayer))
        {
            isFollowingPlayer = true;
            _light.color = Color.red;
            followTimer = followDuration; 
        }
        else
        {
            if (followTimer > 0)
            {
                followTimer -= Time.deltaTime;
            }
            else
            {
                isFollowingPlayer = false;
                _light.color = Color.green;
            }
        }

        if (isFollowingPlayer)
        {
            Transform playerTransform = hit.transform;
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            transform.Rotate(Vector3.up, Random.Range(0f, 360f) * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.up, Random.Range(0f, 360f) * Time.deltaTime);
        }
    }
}