using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    Image Fill;

    // Start is called before the first frame update
    void Start()
    {
        this.Fill = this.transform.Find("Panel").Find("HP").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,Camera.main.transform.rotation * Vector3.up);

    }

    public void UpdateBar(float percentage)
    {

        Fill.fillAmount = percentage;

    }


}
