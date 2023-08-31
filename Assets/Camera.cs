using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform World;
    [SerializeField] private float planetRotate;
    [SerializeField] private float MaxTilt;
    void Start()
    {
       StartCoroutine(WaitForPlanets());
    }
    IEnumerator WaitForPlanets()
    {
        yield return null;
        transform.parent.parent = World.GetChild(0);
        transform.parent.localPosition = Vector3.zero;
        transform.parent.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.parent.GetChild(1).gameObject.SetActive(false);
    }
    private int OnPlanet;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50f)) // Corrected the closing parenthesis here.
            {
                if (hit.transform.parent.parent == World)
                {
                    transform.parent.parent = hit.transform.parent;
                    transform.parent.localPosition = Vector3.zero;
                }
            }
        }
        if(transform.parent.parent != null) transform.localPosition -=  new Vector3(Input.GetAxis("Mouse ScrollWheel") * transform.localPosition.x, 0, 0);
        if(transform.parent.parent != null) transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, transform.parent.parent.GetComponent<Gravity>().r* transform.parent.parent.localScale.x+0.5f, 100000), transform.localPosition.y, transform.localPosition.z);
        // Input.GetAxis.h
        if (Input.GetMouseButton(1))
        {
            float mY = Input.GetAxis("Mouse Y");
            float mX = Input.GetAxis("Mouse X");
            float changeTOPositive = transform.parent.parent.position.x / Mathf.Abs(transform.parent.parent.position.x);
            transform.parent.localEulerAngles += new Vector3(0, (mX + Input.GetAxisRaw("Horizontal")) * changeTOPositive, (mY + Input.GetAxisRaw("Vertical")) * changeTOPositive*-1) * transform.position.x * planetRotate/ transform.parent.parent.GetComponent<Gravity>().r;
            transform.parent.localEulerAngles = new Vector3(0, transform.parent.eulerAngles.y, Mathf.Clamp(transform.parent.eulerAngles.z - (Mathf.Round(transform.parent.eulerAngles.z / 360) * 360), MaxTilt * -1, MaxTilt));
        }

        bool change = false;
        int wasOnPlanet = OnPlanet;
        if (Input.GetKeyDown(KeyCode.E)) { OnPlanet--; change = true; }

        if (Input.GetKeyDown(KeyCode.R)) { OnPlanet++; change = true;  }

        if (change == true)
        {
            if (World.childCount > wasOnPlanet)
            {
                World.GetChild(wasOnPlanet).GetChild(0).gameObject.SetActive(false);
                World.GetChild(wasOnPlanet).GetChild(1).gameObject.SetActive(true);
                World.GetChild(wasOnPlanet).GetChild(2).gameObject.SetActive(false);
            }

            if (World.childCount > OnPlanet & OnPlanet > -1)
            {
                transform.parent.parent = World.GetChild(OnPlanet);
                transform.parent.localPosition = Vector3.zero;

                World.GetChild(OnPlanet).GetChild(0).gameObject.SetActive(true);
                World.GetChild(OnPlanet).GetChild(1).gameObject.SetActive(false);
                World.GetChild(OnPlanet).GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                if (OnPlanet < 0) { OnPlanet = World.childCount-1; }
                else OnPlanet = 0;
                transform.parent.parent = World.GetChild(OnPlanet);
                transform.parent.localPosition = Vector3.zero;

                World.GetChild(OnPlanet).GetChild(0).gameObject.SetActive(true);
                World.GetChild(OnPlanet).GetChild(1).gameObject.SetActive(false);
                World.GetChild(OnPlanet).GetChild(2).gameObject.SetActive(true);

            }
        }


        
    }
}






