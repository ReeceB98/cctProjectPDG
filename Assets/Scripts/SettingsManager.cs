using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle toggle1;
    [SerializeField] private Toggle toggle2;
    [SerializeField] private Toggle toggle3;
    [SerializeField] private Toggle toggle4;
    
    [SerializeField] private GameObject generatingButton1;
    [SerializeField] private GameObject generatingButton2;
    [SerializeField] private GameObject generatingButton3;
    [SerializeField] private GameObject generatingButton4;

    private void Start()
    {
        generatingButton4.SetActive(true);
        generatingButton4.GetComponent<Button>().interactable = false;
        toggle4.isOn = true;
    }

    public void TurnOnSimpleRandomWalkDungeon()
    {
        if (toggle1.isOn == true)
        {
            Debug.Log("Toggle 1 is turned on.");
            generatingButton1.SetActive(true);
            generatingButton2.SetActive(false);
            generatingButton3.SetActive(false);

            //generatingButton1.GetComponent<Button>().interactable = true;
        }
        else
        {
            Debug.Log("Interactable is off");
            //generatingButton1.GetComponent<Button>().interactable = false;
        }
    }
    public void TurnOnRandomWalkWithCorridors()
    {
        if (toggle2.isOn == true)
        {
            Debug.Log("Toggle 2 is turned on.");
            generatingButton1.SetActive(false);
            generatingButton2.SetActive(true);
            generatingButton3.SetActive(false);
        }
        /*else
        {
            generatingButton2.SetActive(false);
        }*/
    }

    public void TurnOnBinarySpaceParitioningDungeon()
    {
        if (toggle3.isOn == true)
        {
            Debug.Log("Toggle 3 is turned on.");
            generatingButton1.SetActive(false);
            generatingButton2.SetActive(false);
            generatingButton3.SetActive(true);
        }
        /*else
        {
            generatingButton3.SetActive(false);
        }*/
    }
}
