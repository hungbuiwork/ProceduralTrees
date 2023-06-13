using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LSystemSettings : MonoBehaviour
{
    static public LSystemSettings Instance = null;
    public float globalScale, globalAngle;
    public LSystemTemplate globalTemplate;
    public List<LSystemTemplate> templates = new List<LSystemTemplate>();
    public int currentTemplateInd = 0;
    public bool usingOverrides = false;
    [SerializeField] private Slider angleSlider, scaleSlider;

    [SerializeField] private TextMeshProUGUI angleText, scaleText, templateText;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            changeTemplate(1);
        }
        setAngle();
        setScale();
        templateText.text = globalTemplate.name;
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
    }

    public void setTemplateDefault()
    {
        //Sets the default 
        scaleSlider.value = globalTemplate.getScaleValue();
        angleSlider.value = globalTemplate.getAngle();
    }
    public void changeTemplate(int i)
    {
        currentTemplateInd += i;
        if (currentTemplateInd < 0) { currentTemplateInd += templates.Count; }
        if (currentTemplateInd > templates.Count - 1) { currentTemplateInd -= templates.Count; }
        globalTemplate = templates[currentTemplateInd];
        templateText.text = globalTemplate.name;
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
