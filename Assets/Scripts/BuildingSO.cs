using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Building")]
public class BuildingSO : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private GameObject prefab;

    public virtual void Create(Vector3 pos) ///OVERRIDE THIS WHEN U CREATE AN INHERITED CLASS
    {
        Debug.Log("Created");
        GameObject newBuilding = Instantiate(prefab, pos, Quaternion.identity);
        newBuilding.transform.parent = BuildingSpawner.Instance.transform;
    }
}
