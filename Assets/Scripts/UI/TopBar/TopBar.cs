using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{

    TextMeshProUGUI ResourceText;
    TextMeshProUGUI MoneyText;
    TextMeshProUGUI BuildingText;
    TextMeshProUGUI UnitText;
    // Start is called before the first frame update
    void Start()
    {

        this.ResourceText = this.transform.Find("Resources").gameObject.GetComponent<TextMeshProUGUI>();
        this.MoneyText = this.transform.Find("Money").gameObject.GetComponent<TextMeshProUGUI>();
        this.BuildingText = this.transform.Find("Buildings").gameObject.GetComponent<TextMeshProUGUI>();
        this.UnitText = this.transform.Find("Units").gameObject.GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateResources(int res, int resMax)
    {

        this.ResourceText.text = res.ToString() + "/" + resMax.ToString();

    }

    public void UpdateMoney(int mon)
    {

        this.MoneyText.text = mon.ToString();

    }

    public void UpdateBuildings(int b, int bMax)
    {

        this.BuildingText.text = b.ToString() + "/" + bMax.ToString();

    }

    public void UpdateUnits(int u, int uMax)
    {

        this.UnitText.text = u.ToString() + "/" + uMax.ToString();

    }



}
