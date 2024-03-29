using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    TilemapVisualizer tilemapVisualizer;

    [SerializeField] private Toggle toggle1;
    [SerializeField] private Toggle toggle2;
    [SerializeField] private Toggle toggle3;
    
    [SerializeField] private GameObject generatingButton1;
    [SerializeField] private GameObject generatingButton2;
    [SerializeField] private GameObject generatingButton3;

    [SerializeField] private GameObject BFSgenerator;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Slider cameraSlider;


    public bool startRW = false;
    public bool startRWC = false;
    public bool startBSP = false;
    public bool isTimerStarting = false;

    public float timeReamining = 0.0f;

    private void Awake()
    {
        tilemapVisualizer = GameObject.FindAnyObjectByType<TilemapVisualizer>();
        cameraSlider.value = 14.0f;
    }

    private void Update()
    {
        TurnOnBinarySpaceParitioningDungeon();
        TurnOnSimpleRandomWalkDungeon();
        TurnOnRandomWalkWithCorridors();
        AdjustCameraSize();
    }

    public void AdjustCameraSize()
    {
        //Debug.Log(cameraSlider.value);
        mainCamera.orthographicSize = cameraSlider.value;
    }

    public void ResetScene()
    {
        ResettingScene();
    }

    private void ResettingScene()
    {
        //tilemapVisualizer.Clear();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void TurnOnSimpleRandomWalkDungeon()
    {
        if (toggle1.isOn == true)
        {
            //Debug.Log("Toggle 1 is turned on.");
            generatingButton1.SetActive(true);
            generatingButton2.SetActive(false);
            generatingButton3.SetActive(false);

            generatingButton1.GetComponent<Button>().interactable = true;
            if (EventSystem.current.currentSelectedGameObject.name == "SimpleRandomWalkDungeonButton")
            {
                startRW = true;
                isTimerStarting = true;
            }
            /*else if (EventSystem.current.currentSelectedGameObject.name == "SimpleRandomWalkDungeonButton" && Input.GetMouseButtonUp(0))
            {
                startRW = true;
                isTimerStarting = false;
            }*/
        }
        else if (toggle1.isOn == false)
        {
            //Debug.Log("Generate Button 1 is off");
            generatingButton1.GetComponent<Button>().interactable = false;
            //tilemapVisualizer.Clear();
        }
    }
    public void TurnOnRandomWalkWithCorridors()
    {
        if (toggle2.isOn == true)
        {
            //Debug.Log("Toggle 2 is turned on.");
            generatingButton1.SetActive(false);
            generatingButton2.SetActive(true);
            generatingButton3.SetActive(false);

            generatingButton2.GetComponent<Button>().interactable = true;
            if (EventSystem.current.currentSelectedGameObject.name == "BFSDungeonButton")
            {
                BFSgenerator.SetActive(true);
                //generatingButton2.GetComponent<Button>().interactable = false;
            }
        }
        else if (toggle2.isOn == false)
        {
            //Debug.Log("Generate Button 2 is off");
            generatingButton2.GetComponent<Button>().interactable = false;
        }
    }

    public void TurnOnBinarySpaceParitioningDungeon()
    {
        if (toggle3.isOn == true)
        {
            //Debug.Log("Toggle 3 is turned on.");
            generatingButton1.SetActive(false);
            generatingButton2.SetActive(false);
            generatingButton3.SetActive(true);

            generatingButton3.GetComponent<Button>().interactable = true;
            if (EventSystem.current.currentSelectedGameObject.name == "RoomsFirstDungeonButton")
            {
                startBSP = true;
            }
        }
        else if (toggle3.isOn == false)
        {
            //Debug.Log("Generate Button 3 is off");
            generatingButton3.GetComponent<Button>().interactable = false;
        }
    }
}
