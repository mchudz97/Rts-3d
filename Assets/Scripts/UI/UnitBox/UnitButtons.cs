using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButtons : MonoBehaviour
{
    // Start is called before the first frame update
    UnitManager Um;
    void Start()
    {
        this.Um = GameObject.FindGameObjectWithTag("UnitManager").GetComponent<UnitManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SoldierBn()
    {

        if (Um.currState == UnitManager.State.SingleSelection)
        {

            this.Um.TryToCreate("Soldier");

        }


    }

    public void TankBn()
    {

        if (Um.currState == UnitManager.State.SingleSelection)
        {

            this.Um.TryToCreate("Tank");

        }

    }

}
