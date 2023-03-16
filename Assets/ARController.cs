using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{

    public GameObject Green;
    public GameObject Blue;
    public GameObject Red;

    [SerializeField]
    private TextMeshProUGUI CounterText;

    [SerializeField]
    private Toggle RedToggle;
    [SerializeField]
    private Toggle BlueToggle;
    [SerializeField]
    private Toggle GreenToggle;


    [SerializeField]
    private float Size;

    private GameObject MyObject;
    public ARRaycastManager RaycastManager;

    List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    Camera arCam;
    GameObject ExistingObject;

    int CubeCount = 0;

    private void Start()
    {
        ExistingObject = null;
        arCam = Camera.main;
        MyObject = Green;
        ChangeSize(0.2f);
    }

    void Update()
    {
        if(Input.touchCount == 0)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if(RaycastManager.Raycast(Input.GetTouch(0).position, Hits))
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began && ExistingObject == null)
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.gameObject.tag == "Kuutio")
                    {
                        ExistingObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpawnPrefab(Hits[0].pose.position);
                        CubeCount++;
                        if (CubeCount >= 15)
                        {
                            CounterText.text = "You Win!";
                        } else
                        {
                            CounterText.text = CubeCount.ToString();
                        }
                    }
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && ExistingObject != null)
            {
                Vector3 ObjectPosition = Hits[0].pose.position;
                ObjectPosition.y += .2f;
                ExistingObject.transform.position = Vector3.MoveTowards(ExistingObject.transform.position, ObjectPosition, 5 * Time.deltaTime);
            }
            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ExistingObject = null;
            }
        }
    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnPosition.y += .2f;
        ExistingObject = Instantiate(MyObject, spawnPosition, Quaternion.identity);
        ExistingObject.transform.localScale = new Vector3(Size, Size, Size);
    }

    public void ChangeColour()
    {
        int i;

        if (GreenToggle.isOn)
        {
            i = 0;
        }
        else if (BlueToggle.isOn)
        {
            i = 1;
        }
        else
        {
            i = 2;
        }

        switch (i)
        {
            case 0:
                MyObject = Green;
                break;
            case 1:
                MyObject = Blue;
                break;
            case 2: 
                MyObject = Red;
                break;
        }
    }

    public void ChangeSize(float NewSize)
    {
        Size = NewSize;
    }

}
