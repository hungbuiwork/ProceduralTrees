using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] static public BuildingSpawner Instance;
    [SerializeField] private List<BuildingSO> options = new List<BuildingSO>();
    [SerializeField] private int currentIndex = 0;

    public void Create(Vector3 pos)
    {
        options[currentIndex].Create(pos);
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
