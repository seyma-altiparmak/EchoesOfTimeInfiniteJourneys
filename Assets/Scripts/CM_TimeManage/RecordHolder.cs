using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordHolder : MonoBehaviour
{
    //Neyin Recordu alýnacaksa ona vereceksin : 

    private List<ReplayRecordSaver> replayRecords = new List<ReplayRecordSaver>();
    private MeshRenderer meshRenderer;
    private Rigidbody rootRigidbody;

    private bool isReplaying;
    private float currentIndex;
    private float indexChangeRate;

    private void Awake()
    {
        rootRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isReplaying = !isReplaying;

            if (isReplaying)
            {
                SetTransform_Mesh(0);
                rootRigidbody.isKinematic = true;
                print("In is Replaying");
            }
            else
            {
                SetTransform_Mesh(replayRecords.Count - 1);
                rootRigidbody.isKinematic = false;
                print("In is !Replaying");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isReplaying)
        {
            ReplayRecordSaver record = new ReplayRecordSaver
            {
                position = transform.position,
                rotation = transform.rotation,
                mesh = meshRenderer
            };
            replayRecords.Add(record);
        }
        else
        {
            float nextIndex = currentIndex + indexChangeRate;

            if (nextIndex < replayRecords.Count && nextIndex >= 0f)
            {
                SetTransform_Mesh(nextIndex);
            }
        }

        indexChangeRate = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            print("In is Replaying D");
            indexChangeRate = 1f;
        }
        if (Input.GetKey(KeyCode.A)){
            print("In is Replaying A");
            indexChangeRate = -1f;
        }
    }

    private void SetTransform_Mesh(float index)
    {
        currentIndex = index;
        ReplayRecordSaver replayRecord = replayRecords[(int)index];

        transform.position = replayRecord.position;
        transform.rotation = replayRecord.rotation;
        meshRenderer = replayRecord.mesh;
    }
}
