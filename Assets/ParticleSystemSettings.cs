using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ParticleSystemType
{
    Campfire,
    Firework

}
[System.Serializable]
public class ParticleSystemSettings : MonoBehaviour
{
    static public ParticleSystemSettings Instance = null;
    public GameObject globalSystem;
    public List<GameObject> systems = new List<GameObject>();
    public int currentSystemInd = 0;
    //private List<GameObject> systemsInScene;

    public bool usingOverrides = false;

    //[SerializeField] private Slider angleSlider, scaleSlider;

    [SerializeField] private TextMeshProUGUI systemText;

    private void Update()
    {
        systemText.text = globalSystem.name;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Only one ParticleSystemSettings can exist!");
            Destroy(this);
        }
        globalSystem = systems[currentSystemInd];
        systemText.text = globalSystem.name;
    }

    public void changeSystem(int i)
    {
        currentSystemInd += i;
        if (currentSystemInd < 0) { currentSystemInd += systems.Count; }
        if (currentSystemInd > systems.Count - 1) { currentSystemInd -= systems.Count; }
        globalSystem = systems[currentSystemInd];
        systemText.text = globalSystem.name;
        setSystemDefault();
    }

    public void setSystemDefault()
    {
        //Sets the default 
        //scaleSlider.value = globalSystem.getScaleValue();
        //angleSlider.value = globalSystem.getAngle();
        //LeafType leaf= globalSystem.getLeaf();
        //setLeaf(leaf);
        globalSystem = systems[currentSystemInd];
        systemText.text = globalSystem.name;

    }

    public void Create(Vector3 pos)
    {
        Instantiate(systems[currentSystemInd],pos, Quaternion.identity);
    }
}
