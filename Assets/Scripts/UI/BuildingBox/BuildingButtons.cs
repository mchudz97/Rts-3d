using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButtons : MonoBehaviour
{
    UnitManager Um;
    void Start()
    {
        this.Um = GameObject.FindGameObjectWithTag("UnitManager").GetComponent<UnitManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BaseBn()
    {

        if(Um.currState == UnitManager.State.NormalMode || Um.currState == UnitManager.State.BuilderMode)
        {
            this.Um.ObjectToBuild = Instantiate(Resources.Load("BaseFinal")) as GameObject;
        }
        

    }

    public void BarracksBn()
    {

        if (Um.currState == UnitManager.State.NormalMode || Um.currState == UnitManager.State.BuilderMode)
        {
            this.Um.ObjectToBuild = Instantiate(Resources.Load("Barracks")) as GameObject;
        }

    }

    public void TowerBn()
    {

        if (Um.currState == UnitManager.State.NormalMode || Um.currState == UnitManager.State.BuilderMode)
        {
            this.Um.ObjectToBuild = Instantiate(Resources.Load("Tower")) as GameObject;
        }

    }

    public void ResourceGathererBn()
    {

        if (Um.currState == UnitManager.State.NormalMode || Um.currState == UnitManager.State.BuilderMode)
        {
            this.Um.ObjectToBuild = Instantiate(Resources.Load("ResourceGatherer")) as GameObject;
        }

    }


}
