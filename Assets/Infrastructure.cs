using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class Infrastructure : MonoBehaviour
{
    [SerializeField]private GameObject DestroyText;
    public bool build;
    [SerializeField] private GameObject CameraHoler;
    [SerializeField] private float GridSize;

    [SerializeField] TextMeshProUGUI TextOre;
    [SerializeField] TextMeshProUGUI TextPopulation;

    [SerializeField] private float PopulationGrouth;
    private int population = 50;
    public static int populationHoused = 100;
    private float ore;
    public static float oreGenerating =1;
    [SerializeField]  public int secondToChangePopulation;
    private bool enoughtOre (float OreNeded)
    {
        if (OreNeded<ore)
        {
            ore -= OreNeded;
            return true;
        }
        else
        {
            return false;
        }
        
    }
    private void Start()
    {
        StartCoroutine(ChangeValues());
    }
    private void SetText()
    {
        TextOre.text = ore.ToString() + "(+"+ oreGenerating.ToString() + "/s)";
        TextPopulation.text = populationHoused.ToString() + "(free seats)/" + population.ToString()+"(population)" + "(+" + Mathf.Round((float)population * PopulationGrouth / secondToChangePopulation).ToString() + "/s)";
    }
    private IEnumerator ChangeValues()
    {
        int loopNumber = 0;
        bool doChangeValues = true;
        while (doChangeValues==true)
        {
            loopNumber++;
            if (loopNumber > secondToChangePopulation)
            {
                loopNumber = 0;
                population += Mathf.RoundToInt((float)population * PopulationGrouth);
            }
            ore += oreGenerating;
            SetText();

            if (population > populationHoused * 2)
            {
                DestroyText.SetActive(true);
                Destroy(gameObject.GetComponent<Infrastructure>());
            }
            yield return new WaitForSeconds(1);
        }
    }
    [System.Serializable]
    private struct BuildObjectData{
        public GameObject ToBuidSelect;
        public GameObject[] ToBuid;
        public float OreCost;
        public int seats;
        public float OreProduction;
    }
    [SerializeField] private BuildObjectData[] BuildObjectsData;
    public void StartBuild(int whatObjectToBuild)
    {
        build = !build;
        if (build == true) StartCoroutine(Build(BuildObjectsData[whatObjectToBuild].ToBuidSelect, BuildObjectsData[whatObjectToBuild].ToBuid, BuildObjectsData[whatObjectToBuild].OreCost, BuildObjectsData[whatObjectToBuild].seats, BuildObjectsData[whatObjectToBuild].OreProduction));
    }

    public IEnumerator Build(GameObject ToBuidSelect, GameObject[] ToBuid,float OreCost, int seats,float  OreProduction)
    {
        GameObject ToBuidSlectCopy = Instantiate(ToBuidSelect);
        while (build ==  true)
        {
            yield return null;
            RaycastHit hit;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f)) // Corrected the closing parenthesis here.
            {
                if (hit.transform == CameraHoler.transform.parent)
                {
                    Vector3 PlanetPosition = CameraHoler.transform.parent.position;
                    ToBuidSlectCopy.transform.position = hit.point;
                    ToBuidSlectCopy.transform.rotation = Quaternion.FromToRotation(-ToBuidSlectCopy.transform.up, (CameraHoler.transform.transform.position - ToBuidSlectCopy.transform.position).normalized) * ToBuidSlectCopy.transform.rotation;
                    ToBuidSlectCopy.transform.parent = CameraHoler.transform.parent.GetChild(0);
                }
            }
            if (Input.GetMouseButtonDown(0) & !EventSystem.current.IsPointerOverGameObject())
            {
                if (enoughtOre(OreCost) == true)
                {
                    GameObject ToBuidCopy = Instantiate(ToBuid[Random.Range(0, ToBuid.Length)]);
                    ToBuidCopy.transform.position = ToBuidSlectCopy.transform.position;
                    ToBuidCopy.transform.rotation = ToBuidSlectCopy.transform.rotation;
                    ToBuidCopy.transform.parent = ToBuidSlectCopy.transform.parent;

                    oreGenerating += OreProduction;
                    populationHoused += seats;
                    CameraHoler.transform.parent.GetComponent<Gravity>().oreGenerationLocal += OreProduction;
                    CameraHoler.transform.parent.GetComponent<Gravity>().freeSeatsLocal += seats;
                    SetText();
                }
            }
        }
        Destroy(ToBuidSlectCopy);
    }
}
