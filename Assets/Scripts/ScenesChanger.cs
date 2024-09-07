using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void StartClkTest()
    {
        SceneManager.LoadScene(2);
    }
    public void mainMenu() {
        SceneManager.LoadScene(0);
    }
}
