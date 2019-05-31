using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    public enum MenuType { Pause, Win, Lose, Load};
    private GameObject game_panel;

    private Text time;
    private Text scores;
    

    private void Start()
    {
        Setup();
        SetupButtons();
    }

    private void Setup()
    {
        game_panel = transform.Find("GamePanel").gameObject;
        game_panel.SetActive(false);

        Transform value = transform.Find("Value");
        time = value.Find("Time").GetComponent<Text>();
        scores = value.Find("Scores").GetComponent<Text>();
        
    }

    private void SetupButtons()
    {
        Transform item_butons = game_panel.transform.Find("Panel/Buttons");
        //back
        Button current_button = item_butons.Find("Back").GetComponent<Button>();
        current_button.onClick.AddListener(() => OpenPanel(MenuType.Pause));
        //level
        current_button = item_butons.Find("Level").GetComponent<Button>();
        current_button.onClick.AddListener(() => RestartLevel());
        //menu
        current_button = item_butons.Find("Menu").GetComponent<Button>();
        current_button.onClick.AddListener(() => { GameController.init.UnloadBundles(); UnityEngine.SceneManagement.SceneManager.LoadScene(0); });

        transform.GetComponentInChildren<Button>().onClick.AddListener(() => OpenPanel(MenuType.Pause));
    }

    private void RestartLevel()
    {
        OpenPanel(MenuType.Pause);
        FindObjectOfType<Board>().ResetBoard();
        GameController.init.ContainAllBricks();
        GameController.init.StartGame();
    }

    public void OpenPanel(MenuType type)
    {
        Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
        game_panel.SetActive((Time.timeScale == 0) ? true : false);

        if (!game_panel.activeSelf) return;

        Text header = game_panel.transform.Find("Panel/Header").GetComponent<Text>();
        Text info_text = game_panel.transform.Find("Panel/Text").GetComponent<Text>();
        info_text.fontSize = 36;
        GameObject back_button = game_panel.transform.Find("Panel/Buttons/Back").gameObject;

        //change name button 
        back_button.transform.parent.Find("Level").GetComponentInChildren<Text>().text = "Restart";

        switch (type)
        {
            case (MenuType.Pause):
                header.text = "Pause";
                info_text.text = string.Empty;
                back_button.SetActive(true);
                break;
            case (MenuType.Win):
                //change name button
                back_button.transform.parent.Find("Level").GetComponentInChildren<Text>().text = "Play";

                header.text = "Good Game";
                info_text.text = string.Format("You score - {0}.\n\n\n Maybe more?",scores.text);
                PlayerPrefs.SetInt("Score", System.Convert.ToInt32(scores.text));
                back_button.SetActive(false);
                break;
            case (MenuType.Lose):
                header.text = "GameOver";
                info_text.text = string.Format("You scores - {0}.\n\n\n Try again!",scores.text);
                back_button.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void RefreshTimeUI(float value)
    {
        if (value <= 5f)
            time.color = Color.red;
        else
            time.color = Color.black;
        time.text = value.ToString("00");
    }

    public void RefreshScoreUI(int i_score)
    {
        scores.text = i_score.ToString();
    }
}
