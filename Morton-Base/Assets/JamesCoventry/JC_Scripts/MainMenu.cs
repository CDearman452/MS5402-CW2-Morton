using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject fadeInPanel;
    public GameObject go_optionsMenu;
    public GameObject go_creditsMenu;
    public GameObject go_mainMenu;

    private void Awake()
    {
        //if (fadeInPanel != null)
        //{
            //GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            //Destroy(panel, 2);
        //}
    }

    void Update()
    {
        if (fadeInPanel.GetComponent<Image>().color.a > 0)
        {
            Color cl_newFade = fadeInPanel.GetComponent<Image>().color;
            cl_newFade.a -= 0.5f * Time.deltaTime;

            fadeInPanel.GetComponent<Image>().color = cl_newFade;
        }
        else fadeInPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void OptionsMenu()
    {
        go_mainMenu.SetActive(false);
        go_optionsMenu.SetActive(true);
    }

    public void CreditsMenu()
    {
        go_mainMenu.SetActive(false);
        go_creditsMenu.SetActive(true);
    }
}


