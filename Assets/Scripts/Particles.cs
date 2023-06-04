using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private List<Rigidbody> particleBodies;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float timeSinceLastSpawned = 0;
    [SerializeField] private float particleLifeTime;
    [SerializeField] private float radiusFromCenter;
    


    private void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
        if (timeSinceLastSpawned > timeBetweenSpawns)
        {
            StartCoroutine(SpawnNewParticle());
            timeSinceLastSpawned = 0f;
        }

        UpdateParticles();
    }

    IEnumerator SpawnNewParticle()
    {
        Debug.Log("Spawning particle");
        Vector3 newVector = Random.insideUnitCircle * radiusFromCenter;
        GameObject newParticle = Instantiate(particlePrefab, this.transform.position + newVector, Quaternion.identity);
        Rigidbody particleRb = newParticle.GetComponent<Rigidbody>();
        particleBodies.Add(particleRb);
        newParticle.transform.parent = this.transform;
        //Edit this to apply forces, etc to each particle
        particleRb.AddForce((particleRb.position - this.transform.position).normalized * 200f);
        particleRb.AddForce(Vector3.up * 500f);
        yield return new WaitForSeconds(particleLifeTime);
        EraseParticle(particleRb);
        
    }

    private void EraseParticle(Rigidbody particle)
    {
        particleBodies.Remove(particle);
        Destroy(particle.gameObject);
    }
    private void UpdateParticles()
    {
        foreach(Rigidbody rb in particleBodies)
        {
            //
        }
    }
}
