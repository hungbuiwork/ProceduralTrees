using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance = null;
    [SerializeField] public Transform indicatorSphere;
    [SerializeField] private List<GameObject> spawned = new List<GameObject>();
    [SerializeField] private Color mainBGColor, newBgColor;
    [SerializeField] private bool dayTime = true;

    public void toggleDaytime()
    {
        if(dayTime)
        {
            dayTime = false;
            Camera.main.backgroundColor = newBgColor;
        }
        else
        {
            dayTime = true;
            Camera.main.backgroundColor = mainBGColor;
        }
    }
    public void addGameObject(GameObject obj)
    {
        spawned.Add(obj);
    }

    public void removeGameObject(GameObject obj)
    {
        spawned.Remove(obj);
        Destroy(obj);
    }

    public void undo()
    {
        GameObject current = spawned[spawned.Count - 1];
        spawned.RemoveAt(spawned.Count - 1);
        Destroy(current);

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        mainBGColor = Camera.main.backgroundColor;
    }
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
