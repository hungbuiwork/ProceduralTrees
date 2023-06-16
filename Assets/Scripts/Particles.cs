using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private List<Rigidbody> particleBodies;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private float timeSinceLastSpawned = 0;
    [SerializeField] private float minParticleLifeTime;
    [SerializeField] private float maxParticleLifeTime;
    [SerializeField] private float radiusFromCenter;
    [SerializeField] private float totalForce;
    [SerializeField] private bool explosion;
    [SerializeField] private int numberOfBurstParticles;
    [SerializeField] private bool firework;

    [SerializeField] private bool destructsAfterLifeTime = false;
    [SerializeField] private float lifeTime = 5.0f;
    private bool firstBurst = true;


    private void Awake()
    {
    }


    private void Update()
    {
        if (firework) {
            StartCoroutine(Firework());
        }
        if (destructsAfterLifeTime)
        {
            Invoke("DestroySelf", lifeTime);
        }
        else {
            if (explosion != true){
                timeSinceLastSpawned += Time.deltaTime;
                if (timeSinceLastSpawned > timeBetweenSpawns)
                {
                    StartCoroutine(SpawnNewParticle());
                    timeSinceLastSpawned = 0f;
                }
            }
            else {
                if (firstBurst == true) {
                    EmitParticleBurst(numberOfBurstParticles);
                    firstBurst = false;
                }
                
            }

            UpdateParticles();
        }
        
    }

    IEnumerator SpawnNewParticle()
    {
        Debug.Log("Spawning particle");
        Vector3 newVector = Random.insideUnitCircle * Random.Range(0,radiusFromCenter);
        GameObject newParticle = Instantiate(particlePrefab, this.transform.position + newVector, Quaternion.identity);
        Rigidbody particleRb = newParticle.GetComponent<Rigidbody>();
        particleBodies.Add(particleRb);
        newParticle.transform.parent = this.transform;
        yield return new WaitForSeconds(Random.Range(minParticleLifeTime,maxParticleLifeTime));
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
            if (explosion == true){
                Vector3 explosionPosition = transform.position;
                rb.AddExplosionForce(totalForce,explosionPosition,1.0f);
            }
            else {
                Vector3 acceleration = Vector3.up * (totalForce / rb.mass);
                rb.velocity = rb.velocity + 0.004f * acceleration;
                rb.position = rb.position + 0.004f * rb.velocity;
                //Old Version:
                //rb.AddForce(Vector3.up * totalForce);
            }
        }
    }

    private void EmitParticleBurst(int numberOfBurstParticles) {
        int counter = numberOfBurstParticles;
        while (counter != 0) {
            StartCoroutine(SpawnNewParticle());
            counter--;
        }
    }

    private void DestroySelf()
    {
        InputManager.Instance.removeGameObject(this.transform.root.gameObject);
    }
    IEnumerator Firework() {
        yield return new WaitForSeconds(2);
        if (firstBurst == true) {
            EmitParticleBurst(numberOfBurstParticles);
            firstBurst = false;
        }
        UpdateParticles();
    }
}
