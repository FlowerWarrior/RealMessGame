using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Security.Cryptography;

public class GameMgr : MonoBehaviour
{
    [SerializeField] Transform pointsHolder;
    [SerializeField] float timeBetweenBlinks;
    [SerializeField] GameObject panelTimer;
    [SerializeField] Slider timerSlider;
    [SerializeField] GameObject textSuccess;
    [SerializeField] GameObject textFail;
    [SerializeField] Light dirLight;
    [SerializeField] GameObject holdPropUI;
    [SerializeField] GameObject[] propPrefabsTemplate;
    [SerializeField] int targetPropCount;
    [SerializeField] GameObject panelTutorial;
    [SerializeField] Material[] skyboxMats;
    [SerializeField] Vector3[] skyboxRots;

    internal List<GameObject> activeProps = new List<GameObject>();
    List<GameObject> targetPropPrefabs = new List<GameObject>();
    List<GameObject> propPrefabs = new List<GameObject>();

    internal static System.Action OnTimeForBlink;
    internal static System.Action PropsReshuffled;
    internal static System.Action PickSuccess;
    internal static System.Action PickFailed;
    internal static GameMgr instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        panelTutorial.SetActive(false);
        panelTimer.SetActive(false);
        textSuccess.SetActive(false);
        textSuccess.SetActive(false);
        ToggleHoldPropUI(false);

        SpawnProps();
        ReshuffleProps();
        ChooseRndPropAsTarget();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        panelTutorial.SetActive(true);
        yield return new WaitForSeconds(4);
        panelTutorial.SetActive(false);
        StartCountdownToBlink();
    }

    private void ChooseRndPropAsTarget()
    {
        activeProps[Random.Range(0, activeProps.Count)].GetComponent<Prop>().isTarget = true;
    }

    float timer= 0f;
    bool isCountdownActive = false;
    private void Update()
    {
        if (isCountdownActive)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                isCountdownActive = false;
                panelTimer.gameObject.SetActive(false);
                OnTimeForBlink?.Invoke();
            }
            timerSlider.value = timer / timeBetweenBlinks;
        }
    }

    public void StartCountdownToBlink()
    {
        panelTimer.gameObject.SetActive(true);
        timer = timeBetweenBlinks;
        isCountdownActive = true;
    }

    private void SpawnProps()
    {
        propPrefabs.AddRange(propPrefabsTemplate);

        for (int i = 0; i < targetPropCount; i++)
        {
            GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Count)];
            targetPropPrefabs.Add(prefab);
            propPrefabs.Remove(prefab);
        }

        for (int i = 0; i < pointsHolder.childCount; i++)
        {
            if (pointsHolder.GetChild(i).gameObject.activeSelf)
            {
                GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Count)];
                activeProps.Add(Instantiate(prefab));
            }
        }
    }

    public void ReshuffleProps()
    {
        GameObject[] arr = activeProps.ToArray();
        System.Random random = new System.Random();
        arr = arr.OrderBy(x => random.Next()).ToArray();
        activeProps.Clear();
        activeProps.AddRange(arr);

        for (int i = 0; i < activeProps.Count; i++)
        {
            if (activeProps[i].GetComponent<Prop>().isTarget == true)
            {
                GameObject oldProp = activeProps[i];
                activeProps[i] = Instantiate(GetNextTargetPrefab());
                activeProps[i].gameObject.GetComponent<Prop>().isTarget = true;
                Destroy(oldProp);
            }

            activeProps[i].transform.position = pointsHolder.GetChild(i).position;
            activeProps[i].transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0,360), 0));
        }

        NextCycleDirLight();
        PropsReshuffled?.Invoke();
    }

    int dirLightCycle = 0;
    private void NextCycleDirLight()
    {
        dirLightCycle++;
        if (dirLightCycle > 3)
            dirLightCycle = 0;

        float newTemp, newIntensity;
        Material newSkybox;
        Vector3 newRot;
        switch (dirLightCycle)
        {
            default:
                newTemp = 2300;
                newIntensity = 1.5f;
                newSkybox = skyboxMats[0];
                newRot = skyboxRots[0];
                break;
            case 1:
                newTemp = 6000;
                newIntensity = 2f;
                newSkybox = skyboxMats[1];
                newRot = skyboxRots[1];
                break;
            case 2:
                newTemp = 13931;
                newIntensity = 0.3f;
                newSkybox = skyboxMats[2];
                newRot = skyboxRots[2];
                break;
        }
        RenderSettings.skybox = newSkybox;
        dirLight.colorTemperature = newTemp;
        dirLight.intensity = newIntensity;
        dirLight.transform.rotation = Quaternion.Euler(newRot);
    }

    public void ToggleHoldPropUI(bool newState)
    {
        holdPropUI.SetActive(newState);
    }

    int cycleTargetPrefab = 0;
    private GameObject GetNextTargetPrefab()
    {
        cycleTargetPrefab++;
        if (cycleTargetPrefab >= targetPropPrefabs.Count)
            cycleTargetPrefab = 0;

        return targetPropPrefabs[cycleTargetPrefab];
    }

    public void ShowTextSuccess()
    {
        ToggleHoldPropUI(false);
        textSuccess.SetActive(true);
        isCountdownActive = false;
    }

    public void ShowTextFail()
    {
        ToggleHoldPropUI(false);
        textFail.SetActive(true);
        isCountdownActive = false;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < pointsHolder.childCount; i++)
        {
            if (pointsHolder.GetChild(i).gameObject.activeSelf)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(pointsHolder.GetChild(i).position, 0.2f);
            }
        }
    }
}
