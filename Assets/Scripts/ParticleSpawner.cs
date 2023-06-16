using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] static public ParticleSpawner Instance;
    [SerializeField] private List<GameObject> options = new List<GameObject>();
    [SerializeField] private int currentIndex = 0;

    public void Create(Vector3 pos)
    {
        GameObject newParticle = Instantiate(options[currentIndex],pos, Quaternion.identity);
        InputManager.Instance.addGameObject(newParticle);
    }

    private void Update()
    {
        scrollOption((int) Input.mouseScrollDelta.y);
    }
    void scrollOption(int amount)
    {
        currentIndex = (currentIndex + amount) % options.Count;
        if (currentIndex < 0) { currentIndex += options.Count; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("NOOOO");
            Destroy(this);
        }
    }
}
