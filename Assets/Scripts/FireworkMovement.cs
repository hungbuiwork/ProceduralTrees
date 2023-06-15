using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkMovement : MonoBehaviour
{
    private int count = 0;

    // Update is called once per frame
    void Update()
    {
        Rigidbody emitterRb = this.gameObject.GetComponent<Rigidbody>();
        if (count < 3) {
            emitterRb.AddForce(Vector3.up * 300f);
            
        }
        count++;

        if (count >= 360) {
            this.gameObject.transform.GetChild(0).localScale = Vector3.zero;
        }
    }
}
