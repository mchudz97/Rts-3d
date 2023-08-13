using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBot : MonoBehaviour
{

    List<GameObject> Buildings;
    List<GameObject> Units;
    List<Step> stepsToDo;
    List<GameObject> SelectedUnits;
    int money;
    int resources;
    int resourceLimit;
    int unitLimit;
    GameObject target;
    public GameObject Base;
    public GameObject toBuild;
    public GameObject toCreate;

    float range = 25;

    float timer = 0;
   

    // Start is called before the first frame update
    void Start()
    {

        this.money = 100;
        this.resourceLimit = 200;
        this.unitLimit = 10;
        this.SelectedUnits = new List<GameObject>(0);
        this.Buildings = new List<GameObject>(0);
        this.Units = new List<GameObject>(0);
        this.stepsToDo = new List<Step>()
        {

            new Step(2, 1, 2, 5, 0),
            new Step(4, 2, 3, 10, 0),
            new Step(5, 2, 4, 10, 2),
            new Step(5, 4, 5, 15, 2),
            new Step(7, 5, 8, 15, 8),
            new Step(7, 5, 8, 20, 15)

        };

        this.Base = Instantiate(Resources.Load("BaseFinal")) as GameObject;
    

    }

    // Update is called once per frame
    void Update()
    {



        this.Cleanup();

        if (this.Base == null)
        {

            SceneManager.LoadScene(2);
            return;

        }

        if ( !this.Base.GetComponent<Building>().IsBuilded )
        {

            this.Base.GetComponent<Building>().TryToBuild(new Vector3(-59.71049f, 0.49f, 63.91415f), true);
            this.Buildings.Add(this.Base);
            this.Base.tag = "EnemyUnits";

        }



        this.StepChecker();
        this.GatherProfits();
        this.timer += Time.deltaTime;
        DetectAttack();
        AttackEnemy();

    }



    public void StepChecker()
    {     

        if (this.stepsToDo.Count > 1)
        {

            if (!this.stepsToDo[0].StepPassed(this.getResBuildingsCount(), this.getBarracksCount(), this.getTowerCount(), this.getSoldierCount(), this.getTankCount()))
            {

                this.TryToCompleteStep();

            }
            else
            {

                this.stepsToDo.RemoveAt(0);
                this.SelectedUnits = new List<GameObject>(this.Units);

            }

        }
        else
        {

            if (!this.stepsToDo[0].StepPassed(this.getResBuildingsCount(), this.getBarracksCount(), this.getTowerCount(), this.getSoldierCount(), this.getTankCount()))
            {

                this.TryToCompleteStep();

            }
            else
            {

                this.SelectedUnits = new List<GameObject>(this.Units);

            }
            
        }  

    }


    void GatherProfits()
    {

        if(this.timer >= 10)
        {

            this.money += 50;
            foreach(GameObject b in this.Buildings)
            {

                if(b != null && b.GetComponent<Building>().BuildingType == Building.TYPE.RESOURCES && this.resources < this.resourceLimit)
                {

                    this.resources += 10;

                }

            }

            this.timer = 0;

        }



    }

    void TryToCompleteStep()
    {

        if(toBuild != null) { 

            TryToBuild();
            return;

        }
        if(toCreate != null)
        {

            TryToCreate();
            return;

        }
        


        if (this.stepsToDo[0].resourcesToBuild > this.getResBuildingsCount() && this.money >= 200)
        {
            
            toBuild = Instantiate(Resources.Load("ResourceGatherer")) as GameObject;

        }
        else if(this.stepsToDo[0].towersToBuild > this.getTowerCount() && this.money >= 150  && this.resources >= 40)
        {

            toBuild = Instantiate(Resources.Load("Tower")) as GameObject;

        }
        else if(this.stepsToDo[0].barracksToBuild > this.getBarracksCount() && this.money >= 200 && this.resources >= 100)
        {

            toBuild = Instantiate(Resources.Load("Barracks")) as GameObject;

        }
        else if(this.stepsToDo[0].soldiersToCreate > this.getSoldierCount() && this.money >= 40 && this.getBarracksCount() != 0 )
        {

            toCreate = Instantiate(Resources.Load("Soldier")) as GameObject;

        }
        else if(this.stepsToDo[0].tanksToCreate > this.getTankCount() && this.money >= 150 && this.resources >= 100 && this.getBarracksCount() != 0)
        {

            toCreate = Instantiate(Resources.Load("Tank")) as GameObject;

        }

        if(toBuild != null)
        {

            toBuild.transform.position = new Vector3(0, -10, 0);

        }


    }

    IEnumerator Waiting()
    {

        yield return new WaitForSecondsRealtime(2);

    }

    bool TryToBuild()
    {

        StartCoroutine(Waiting());

        var bScript = toBuild.GetComponent<Building>();

        if (bScript != null && bScript.MoneyPrice <= this.money && bScript.ResourcePrice <= this.resources)
        {

            var buildPos = this.toBuild.transform.position;

            Vector3 randPlace = new Vector3(Random.Range(-range, range) + Base.transform.position.x,
                Base.transform.position.y, Random.Range(-range, range) + Base.transform.position.z);

            if (!Physics.Linecast(new Vector3(buildPos.x, buildPos.y + 1, buildPos.z),
                new Vector3(buildPos.x, buildPos.y - 1, buildPos.z), LayerMask.GetMask("SUPERLEJER")))
            {

                this.toBuild.transform.position = randPlace;
                return false;

            }

            foreach (GameObject b in this.Buildings)
            {

                if (Vector3.Distance(b.transform.position, buildPos) < 8)
                {

                    this.toBuild.transform.position = randPlace;
                    return false;

                }

            }
            if (bScript.IsSurfaceAvailable())
            {

                    
                    
                bScript.Build(buildPos);
                toBuild.tag = "EnemyUnits";
                this.Buildings.Add(toBuild);
                this.money -= bScript.MoneyPrice;
                this.resources -= bScript.ResourcePrice;
                toBuild = null;
                return true;


            }
            else
            {

                this.toBuild.transform.position = randPlace;

            }

        }
        
        return false;

    }

    public void TryToCreate()
    {

        var uScript = toCreate.GetComponent<MyUnit>();

        if(uScript != null && uScript.MoneyCost <= this.money && uScript.ResourceCost <= this.resources)
        {
            
            foreach(GameObject b in this.Buildings)
            {

                if(b.GetComponent<Barracks>()!= null)
                {

                    uScript.Agent.enabled = false;
                    toCreate.transform.position = b.transform.position;
                    uScript.Agent.enabled = true;
                    toCreate.GetComponent<MyUnit>().MoveTo(toCreate.transform.position); // check
                    toCreate.tag = "EnemyUnits";
                    this.Units.Add(toCreate);
                    this.resources -= uScript.ResourceCost;
                    this.money -= uScript.MoneyCost;
                    toCreate = null;
                    return;

                }

            }
            
        }

    }



    void Cleanup()
    {

        var newBuildings = new List<GameObject>(0);
        foreach(GameObject b in this.Buildings)
        {

            if(b != null)
            {

                newBuildings.Add(b);

            }

        }

        bool changed = this.Buildings.Count != newBuildings.Count;
        if (changed)
        {

            this.Buildings = newBuildings;
            this.UpdateBuildingBenefits();

        }

        var newSelected = new List<GameObject>(0);

        foreach(GameObject u in this.SelectedUnits)
        {

            if(u!= null)
            {

                newSelected.Add(u);

            }

        }

        this.SelectedUnits = newSelected;

        var newUnits = new List<GameObject>(0);
        foreach(GameObject u in this.Units)
        {

            if(u != null)
            {

                newUnits.Add(u);

            }

        }

        this.Units = newUnits;


    }
    void UpdateBuildingBenefits()
    {

        foreach(GameObject b in this.Buildings)
        {

            int uLim = 10;
            int resLim = 200;
            var bType = b.GetComponent<Building>().BuildingType;
            if (bType == Building.TYPE.FACTORY)
            {

                uLim += 5;

            }
            if(bType == Building.TYPE.RESOURCES)
            {

                resLim += 100;

            }

            this.resourceLimit = resLim;
            this.unitLimit = uLim;

        } 

    }

    public int getResBuildingsCount()
    {
        int i = 0;
        foreach(GameObject b in this.Buildings)
        {

            if(b!=null && b.GetComponent<Building>().BuildingType == Building.TYPE.RESOURCES)
            {

                i++;

            }

        }

        return i;

    }

    public int getBarracksCount()
    {
        int i = 0;
        foreach (GameObject b in this.Buildings)
        {

            if (b != null && b.GetComponent<Building>().BuildingType == Building.TYPE.FACTORY)
            {

                i++;

            }

        }

        return i;

    }
    public int getTowerCount()
    {

        int i = 0;
        foreach (GameObject b in this.Buildings)
        {

            if (b != null && b.GetComponent<Building>().BuildingType == Building.TYPE.DEFENSIVE)
            {

                i++;

            }

        }

        return i;

    }
    public int getSoldierCount()
    {

        int i = 0;
        foreach (GameObject u in this.Units)
        {

            if (u != null && u.GetComponent<MyUnit>().UnitType == MyUnit.TYPE.SOLDIER)
            {

                i++;

            }

        }

        return i;

    }
    public int getTankCount()
    {

        int i = 0;
        foreach (GameObject u in this.Units)
        {

            if (u != null && u.GetComponent<MyUnit>().UnitType == MyUnit.TYPE.TANK)
            {

                i++;

            }

        }

        return i;

    }


    public void AttackEnemy()
    {

        if(this.target == null)
        {

            this.target = this.GetNearestEnemy();

        }

        foreach(GameObject unit in this.SelectedUnits)
        {

            var unitScript = unit.GetComponent<MyUnit>();

            if(unitScript != null)
            {
                if(unitScript.Target != null && unitScript.Target != target)
                {

                    this.target = unitScript.Target;

                }
                unitScript.Attack(target);

            }

        }



    }
    public void DetectAttack()
    {

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("MineUnits"))
        {

            if(Vector3.Distance(this.Base.transform.position, enemy.transform.position) <= 50)
            {

                foreach(GameObject unit in this.Units)
                {

                    unit.GetComponent<MyUnit>().Attack(enemy);

                }

            }


        }


    }

    public GameObject GetNearestEnemy()
    {

        float maxDist = 0;
        GameObject possibleTarget = null;

        foreach(GameObject e in GameObject.FindGameObjectsWithTag("MineUnits"))
        {

            var dist = Vector3.Distance(this.Base.transform.position, e.transform.position);

            if(dist > maxDist)
            {

                possibleTarget = e;
                maxDist = dist;

            }


        }


        return possibleTarget;

    }




}
