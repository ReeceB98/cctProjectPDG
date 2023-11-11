using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void ButtonPressedToGenerateRoom()
    {
        Debug.Log("Generate Room Button Pressed.");
        SceneManager.LoadScene(0);
    }

}
