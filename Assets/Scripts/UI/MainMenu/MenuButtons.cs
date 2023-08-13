using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnStartButton()
    {

        SceneManager.LoadScene(1);


    }
    public void OnQuitButton()
    {

        Application.Quit();

    }

    public void OnOptionsButton()
    {

        this.transform.Find("MainPanel").gameObject.SetActive(false);
        this.transform.Find("OptionPanel").gameObject.SetActive(true);

    }

    public void OnFHDResButton()
    {

        Screen.SetResolution(1920, 1080, true);


    }
    public void OnHDResButton()
    {

        Screen.SetResolution(1280, 720, true);

    }
    public void SetLowQuality()
    {

        QualitySettings.SetQualityLevel(0);
        Debug.Log(QualitySettings.currentLevel);

    }

    public void SetHighQuality()
    {

        QualitySettings.SetQualityLevel(3);
        Debug.Log(QualitySettings.currentLevel);

    }
    public void OnBackButton()
    {

        
        this.transform.Find("OptionPanel").gameObject.SetActive(false);
        this.transform.Find("MainPanel").gameObject.SetActive(true);

    }



}
