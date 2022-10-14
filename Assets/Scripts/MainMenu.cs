using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Toggle[] toggle;
    Toggle[] toggle2;

    public void Start()
    {
        toggle = GameObject.FindGameObjectWithTag("BoardSize").GetComponentsInChildren<Toggle>();
        toggle2 = GameObject.FindGameObjectWithTag("PiecesChoice").GetComponentsInChildren<Toggle>();
        toggle[Values.board_size - 4].isOn = true;
        toggle2[Values.choice - 1].isOn = true;
    }
    public void BoardSelected()
    {
        toggle = GameObject.FindGameObjectWithTag("BoardSize").GetComponentsInChildren<Toggle>();
        if (toggle[0].isOn == true)
        {
            Values.board_size = 4;
            Values.board_size_multiplier = 1.33f;
            Values.board_size_subtructor = -1.98f;
            Values.piece_size = 6.0f;
        }
        else if (toggle[1].isOn == true)
        {
            Values.board_size = 5;
            Values.board_size_multiplier = 1.0625f;
            Values.board_size_subtructor = -2.1f;
            Values.piece_size = 5.0f;
        }
        else if (toggle[2].isOn == true)
        {
            Values.board_size = 6;
            Values.board_size_multiplier = 0.89f;
            Values.board_size_subtructor = -2.2f;
            Values.piece_size = 4.0f;

        }
        else if (toggle[3].isOn == true)
        {
            Values.board_size = 7;
            Values.board_size_multiplier = 0.7583f;
            Values.board_size_subtructor = -2.25f;
            Values.piece_size = 3.5f;
        }
        else if (toggle[4].isOn == true)
        {
            Values.board_size = 8;
            Values.board_size_multiplier = 0.66f;
            Values.board_size_subtructor = -2.3f;
            Values.piece_size = 3.0f;
        }
    }
    public void Choice()
    {
        toggle2 = GameObject.FindGameObjectWithTag("PiecesChoice").GetComponentsInChildren<Toggle>();
        if (toggle2[0].isOn == true)
        {
            Values.choice = 1;
        }
        else if (toggle2[1].isOn == true)
        {
            Values.choice = 2;
        }
    }
    public void PlayGame()
    {
        Values.solution = false;
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Solution()
    {
        Values.solution = true;
        SceneManager.LoadScene("Game");
    }
}