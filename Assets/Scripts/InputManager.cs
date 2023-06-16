using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] public Transform indicatorSphere;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("DOING");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200f, 1 << 6))
            {
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                indicatorSphere.position = hit.point;



                Spawner.Instance.Create(hit.point);
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("DOING");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200f, 1 << 6))
            {
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                indicatorSphere.position = hit.point;



                ParticleSystemSettings.Instance.Create(hit.point);
            }

        }

    }
}
