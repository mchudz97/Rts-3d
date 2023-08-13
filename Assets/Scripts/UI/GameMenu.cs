using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 1f;

    public Slider Slider;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Slide()
    {

        this.speed = Slider.value;
        this.transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = "Speed: " + (int) this.speed;

    }

    public void Resume()
    {

        Time.timeScale = speed;
        this.transform.parent.gameObject.SetActive(false);

    }

    public void Pause()
    {

        Time.timeScale = 0f;

    }

    public void OnQuitButtonPressed()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }




}
