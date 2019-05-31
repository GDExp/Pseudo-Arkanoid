using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        Time.timeScale = 1;
        transform.Find("Buttons/Quad").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ChoseLevel("Bricks_1"));
        transform.Find("Buttons/Oval").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ChoseLevel("Bricks_2"));
    }
    //in new scene GameController load assets bundle
    private void ChoseLevel(string type_briks)
    {
        PlayerPrefs.SetString("Type", type_briks);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
