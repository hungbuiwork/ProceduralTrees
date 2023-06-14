using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum LeafType
{
    Sunflower,
    Purpleflower,
    Grass,
    Leaf1

}

[System.Serializable]
public class Leaf
{
    public LeafType leafType;
    public GameObject prefab;
}
public class LSystemSettings : MonoBehaviour
{
    static public LSystemSettings Instance = null;
    public float globalScale, globalAngle;
    public LSystemTemplate globalTemplate;
    public List<LSystemTemplate> templates = new List<LSystemTemplate>();
    public int currentTemplateInd = 0;

    public GameObject globalLeaf;
    public List<Leaf> leafMapping= new List<Leaf>();
    public int currentLeafInd = 0;

    public bool usingOverrides = false;

    [SerializeField] private Slider angleSlider, scaleSlider;

    [SerializeField] private TextMeshProUGUI angleText, scaleText, templateText, leafText;
    

    private void Update()
    {
        setAngle();
        setScale();
        templateText.text = globalTemplate.name;
        leafText.text = leafMapping[currentLeafInd].leafType.ToString();
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Only one LSystemSettings can exist!");
            Destroy(this);
        }
        globalTemplate = templates[currentTemplateInd];
        globalLeaf = leafMapping[currentLeafInd].prefab;
    }

    public void setTemplateDefault()
    {
        //Sets the default 
        scaleSlider.value = globalTemplate.getScaleValue();
        angleSlider.value = globalTemplate.getAngle();
        LeafType leaf= globalTemplate.getLeaf();
        setLeaf(leaf);

    }

    

    public void setLeaf(LeafType leaf)
    {
        for(int i = 0; i < leafMapping.Count; i++)
        {
            if (leafMapping[i].leafType == leaf)
            {
                currentLeafInd= i;
                globalLeaf = leafMapping[i].prefab;
                break;
            }
        }
    }
    public void changeTemplate(int i)
    {
        currentTemplateInd += i;
        if (currentTemplateInd < 0) { currentTemplateInd += templates.Count; }
        if (currentTemplateInd > templates.Count - 1) { currentTemplateInd -= templates.Count; }
        globalTemplate = templates[currentTemplateInd];
        templateText.text = globalTemplate.name;
        setTemplateDefault();
    }

    public void changeLeaf(int i)
    {
        currentLeafInd += i;
        if (currentLeafInd < 0) { currentLeafInd += leafMapping.Count; }
        if (currentLeafInd > leafMapping.Count - 1) { currentLeafInd -= leafMapping.Count; }
        globalLeaf = leafMapping[currentLeafInd].prefab;
        leafText.text = leafMapping[currentLeafInd].leafType.ToString();
    }

    public void setScale()
    {
        globalScale = scaleSlider.value;
        scaleText.text = globalScale.ToString(".00");
    }
    public void setAngle()
    {
        globalAngle = angleSlider.value;
        angleText.text = angleSlider.value.ToString() + "°";
    }

    public void setTemplate(LSystemTemplate template)
    {
        globalTemplate = template;
    }





}
