using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitManager : MonoBehaviour
{
    public enum State { NormalMode = 0, BuilderMode, SingleSelection, MultipleSelection };

    List<GameObject> Buildings;

    List<GameObject> Units;

    List<GameObject> selectedUnits;
    public GameObject selectedBuilding;

    GameObject BottomPanel;
    public State currState { get; set; }

    GameObject Base;
    bool isBaseBuilded = false;
    bool selectionEnded = true;

    PlayerCam playerCam;
    TopBar TopBar;
    int money { get; set; }
    int unitLimit { get; set; }
    int buildingLimit { get; set; }
    int resourceLimit { get; set; }
    int resources { get; set; }

    float cooldown = 10;
    float timer = 0;
    float inputDelay;

    public GameObject ObjectToBuild;
    Building toBuild;
    GameObject Selector;
    public GameObject GameMenu;

    // Start is called before the first frame update
    void Start()
    {

        this.Buildings = new List<GameObject>(0);
        this.Units = new List<GameObject>(0);
        this.selectedUnits = new List<GameObject>(0);
        this.currState = State.NormalMode;
        this.playerCam = GameObject.FindGameObjectWithTag("MyCam").GetComponent<PlayerCam>();
        this.TopBar = GameObject.FindGameObjectWithTag("TopBar").GetComponent<TopBar>();
        this.Selector = GameObject.FindGameObjectWithTag("Selector");
        this.Selector.SetActive(false);
        this.BottomPanel = GameObject.FindGameObjectWithTag("BottomPanel");
        this.buildingLimit = 20;
        this.resourceLimit = 200;
        this.money = 100;
        this.resources = 0;
        this.unitLimit = 10;
        /*TopBar.UpdateResources(this.resources, this.resourceLimit);
        TopBar.UpdateBuildings(this.Buildings.Count, this.buildingLimit);
        TopBar.UpdateMoney(this.money);
        TopBar.UpdateUnits(this.Units.Count, this.unitLimit);*/
        


    }

    // Update is called once per frame
    void Update()
    {

        this.Cleanup();
        this.timer += Time.deltaTime;

        GatherProfits();
        StateManager();

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            this.GameMenuShow();

        }

    }


    void Build(Building building, bool clicked)
    {

        var pos = playerCam.GetMapCordsFromMouse();



        if (pos != null)
        {

            if (building.TryToBuild((Vector3)pos, clicked))
            {

                ObjectToBuild.tag = "MineUnits";
                this.Buildings.Add(building.transform.gameObject);

                

                if (!isBaseBuilded)
                {
                    isBaseBuilded = true;
                    this.Base = ObjectToBuild;

                }

                this.money -= building.MoneyPrice;
                this.resources -= building.ResourcePrice;
                this.TopBar.UpdateMoney(this.money);
                this.TopBar.UpdateResources(this.resources, this.resourceLimit);
                this.TopBar.UpdateBuildings(Buildings.Count, buildingLimit);
                this.currState = State.NormalMode;
                this.ObjectToBuild = null;
                this.toBuild = null;

            }

        }

    }

    void StateManager()
    {

        if (currState == State.BuilderMode)
        {

            this.AtBuilderState();

        }
        else if(currState == State.SingleSelection)
        {

            this.AtSingleSelectionState();

        }
        else if(currState == State.MultipleSelection)
        {

            this.AtMultipleSelectionState();

        }
        else if (currState == State.NormalMode)
        {

            this.AtNormalState();

        }


    }

    public void UnsellectAll()
    {

        if (selectedBuilding != null)
        {

            this.selectedBuilding.GetComponent<Building>().Unselect();
            this.selectedBuilding = null;
            return;

        }

        foreach ( GameObject unit in selectedUnits)
        {

            unit.GetComponent<MyUnit>().Unselect();

        }

        this.selectedUnits.Clear();

    }

    public void SelectFromBox(RectTransform box)
    {

        var rectStart = box.anchoredPosition - box.sizeDelta/2;
        var rectEnd = box.anchoredPosition + box.sizeDelta/2;
        
        foreach(GameObject unit in Units)
        {

            var posOnScreen = Camera.main.WorldToScreenPoint(unit.transform.position);

            if(posOnScreen.x <= rectEnd.x && posOnScreen.y <= rectEnd.y && posOnScreen.x >= rectStart.x && posOnScreen.y >= rectStart.y)
            {

                unit.GetComponent<MyUnit>().Select();
                this.selectedUnits.Add(unit);

            }

        } 

    }

    public void TryToCreate(string unit)
    {

        if(this.selectedBuilding != null && this.selectedBuilding.GetComponent<Building>().BuildingType == Building.TYPE.FACTORY)
        {

            var toCreate = Instantiate(Resources.Load(unit)) as GameObject;
            var toCreateScript = toCreate.GetComponent<MyUnit>();

            if (this.resources >= toCreateScript.ResourceCost && this.money >= toCreateScript.MoneyCost && this.Units.Count < this.unitLimit)
            {

                toCreate.GetComponent<MyUnit>().Agent.enabled = false;
                toCreate.transform.position = this.selectedBuilding.transform.position;
                toCreate.GetComponent<MyUnit>().Agent.enabled = true;
                toCreate.tag = "MineUnits";
                this.Units.Add(toCreate);
                this.resources -= toCreateScript.ResourceCost;
                this.money -= toCreateScript.MoneyCost;

            }
            else
            {
                Destroy(toCreate);
            }

        }

    }

    public void GatherProfits()
    {

        if(this.timer >= this.cooldown)
        {
            this.timer = 0;
            this.money += 50;


            foreach(GameObject b in this.Buildings)
            {
                if(b != null && b.GetComponent<Building>().BuildingType == Building.TYPE.RESOURCES && this.resources < this.resourceLimit)
                {
                    if(resources<= resourceLimit)
                    {

                        this.resources += 10;

                    }
                    
                }

            }

        }

    }

    public void Cleanup()
    {

        if(this.Base == null && isBaseBuilded)
        {

            SceneManager.LoadScene(3);

        }

        var newBuildings = new List<GameObject>(0);
        foreach (GameObject b in this.Buildings)
        {

            if (b != null)
            {

                newBuildings.Add(b);

            }

        }

        this.Buildings = newBuildings;
        this.UpdateBuildingBenefits();
        var newUnits = new List<GameObject>(0);

        foreach (GameObject u in this.Units)
        {

            if (u != null)
            {

                newUnits.Add(u);

            }

        }

        this.Units = newUnits;
        
        var newSelected = new List<GameObject>(0);

        foreach(GameObject u in this.selectedUnits)
        {

            if(u != null)
            {

                newSelected.Add(u);

            }


        }

        this.selectedUnits = newSelected;

        if (selectedBuilding == null)
        {

            selectedBuilding = null;

        }

        this.TopBar.UpdateBuildings(this.Buildings.Count, this.buildingLimit);
        this.TopBar.UpdateMoney(this.money);
        this.TopBar.UpdateResources(this.resources, this.resourceLimit);
        this.TopBar.UpdateUnits(this.Units.Count, this.unitLimit);

    }

    public void UpdateBuildingBenefits()
    {

        int uLim = 10;
        int resLim = 200;

        foreach(GameObject b in this.Buildings)
        {

            var bType = b.GetComponent<Building>().BuildingType;

            if (bType == Building.TYPE.RESOURCES)
            {

                resLim += 100;

            }
            if (bType == Building.TYPE.FACTORY)
            {

                uLim += 5;

            }

        }

        this.resourceLimit = resLim;
        this.unitLimit = uLim;

    }

    public void AtBuilderState()
    {
       
        bool Bclick = false;

        if (Input.anyKeyDown)
        {

            if (!Input.GetMouseButtonDown(0))
            {

                currState = State.NormalMode;
                Destroy(ObjectToBuild);
                toBuild = null;
                return;

            }
            else
            {

                Bclick = true;

            }

        }


        Build(toBuild, Bclick);

    }

    public void AtSingleSelectionState()
    {

        if (Input.GetMouseButtonDown(1))
        {

            this.UnsellectAll();
            this.currState = State.NormalMode;
            var ubox = this.BottomPanel.transform.Find("UnitBox");
            if (ubox.gameObject.activeInHierarchy)
            {
                ubox.gameObject.SetActive(false);

            }

            var box = this.BottomPanel.transform.Find("BuildingBox");
            if (!box.gameObject.activeInHierarchy)
            {

                box.gameObject.SetActive(true);

            }


            return;

        }
        else if (Input.GetMouseButton(1) && inputDelay <= 0)
        {

            this.Selector.SetActive(true);
            this.Selector.GetComponent<Selector>().StartPos = Input.mousePosition;
            this.selectionEnded = false;
            this.currState = State.MultipleSelection;

        }
        else if (inputDelay <= 0)
        {

            GameObject singleTar;

            if (this.selectedBuilding == null && this.selectedUnits.Count == 0)
            {

                if ((singleTar = playerCam.SingleTarget()) != null)
                {

                    Building bSelected;
                    MyUnit uSelected;



                    if ((bSelected = singleTar.GetComponent<Building>()) != null)
                    {

                        bSelected.Select();
                        this.selectedBuilding = singleTar;
                        var box = this.BottomPanel.transform.Find("BuildingBox");
                        if (box.gameObject.activeInHierarchy)
                        {

                            box.gameObject.SetActive(false);
                            if (bSelected.BuildingType == Building.TYPE.FACTORY)
                            {

                                var ubox = this.BottomPanel.transform.Find("UnitBox");
                                if (!ubox.gameObject.activeInHierarchy)
                                {

                                    ubox.gameObject.SetActive(true);

                                }

                            }

                        }


                    }
                    else if ((uSelected = singleTar.GetComponent<MyUnit>()) != null)
                    {

                        uSelected.Select();
                        this.selectedUnits.Add(singleTar);


                    }
                    else
                    {

                        return;

                    }

                }

            }

            if (this.selectedUnits.Count != 0)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    var enemy = this.playerCam.getEnemy(Input.mousePosition);
                    if (enemy == null)
                    {
                        var cord = this.playerCam.GetCord();
                        if (cord != null)
                        {

                            this.selectedUnits[0].GetComponent<MyUnit>().MoveTo((Vector3)cord);
                            selectedUnits[0].GetComponent<MyUnit>().Agent.stoppingDistance = 4;

                        }

                    }
                    else
                    {

                        this.selectedUnits[0].GetComponent<MyUnit>().Target = enemy;
                        selectedUnits[0].GetComponent<MyUnit>().Agent.stoppingDistance = 4;

                    }

                }

            }



        }

        inputDelay -= Time.deltaTime;

    }

    public void AtMultipleSelectionState()
    {

        if (Input.GetMouseButtonDown(1))
        {

            this.UnsellectAll();
            this.Selector.GetComponent<Selector>().Box.sizeDelta = new Vector2(0, 0);
            this.Selector.SetActive(false);
            this.selectionEnded = true;
            this.currState = State.NormalMode;
            return;

        }
        else if (Input.GetMouseButton(1))
        {

            this.Selector.GetComponent<Selector>().StretchTo(Input.mousePosition);

        }
        else
        {

            if (!selectionEnded)
            {

                this.SelectFromBox(this.Selector.GetComponent<Selector>().Box);
                this.Selector.GetComponent<Selector>().Box.sizeDelta = new Vector2(0, 0);
                this.Selector.SetActive(false);
                this.selectionEnded = true;

                if (selectedUnits.Count == 0)
                {

                    this.currState = State.NormalMode;

                }
            }

            if (Input.GetMouseButtonDown(0))
            {

                var enemy = this.playerCam.getEnemy(Input.mousePosition);



                if (enemy == null)
                {

                    var cord = this.playerCam.GetCord();
                    if (cord != null)
                    {

                        foreach (GameObject unit in this.selectedUnits)
                        {

                            unit.GetComponent<MyUnit>().MoveTo((Vector3)cord);
                            unit.GetComponent<MyUnit>().Agent.stoppingDistance = this.selectedUnits.Count / 3;

                        }

                    }

                }
                else
                {

                    foreach (GameObject unit in this.selectedUnits)
                    {

                        unit.GetComponent<MyUnit>().Target = enemy;
                        unit.GetComponent<MyUnit>().Agent.stoppingDistance = this.selectedUnits.Count / 3;

                    }


                }


            }


        }

    }

    public void AtNormalState()
    {

        if (ObjectToBuild != null)
        {

            if (ObjectToBuild.name.Contains("Base"))
            {

                if (!isBaseBuilded)
                {

                    toBuild = ObjectToBuild.GetComponent<Base>();

                }
                else
                {

                    Destroy(ObjectToBuild);
                    return;

                }

            }
            else if (ObjectToBuild.name.Contains("Barracks"))
            {

                var tmp = ObjectToBuild.GetComponent<Barracks>();

                if (isBaseBuilded && tmp.ResourcePrice <= this.resources && tmp.MoneyPrice <= this.money && this.Buildings.Count < this.buildingLimit)
                {

                    toBuild = tmp;

                }
                else
                {

                    Destroy(ObjectToBuild);
                    return;

                }

            }
            else if (ObjectToBuild.name.Contains("Tower"))
            {

                var tmp = ObjectToBuild.GetComponent<Tower>();


                if (isBaseBuilded && tmp.ResourcePrice <= this.resources && tmp.MoneyPrice <= this.money && this.Buildings.Count < this.buildingLimit)
                {

                    toBuild = tmp;

                }
                else
                {

                    Destroy(ObjectToBuild);
                    return;

                }

            }
            else if (ObjectToBuild.name.Contains("Resource"))
            {

                var tmp = ObjectToBuild.GetComponent<ResourceGatherer>();

                if (isBaseBuilded && tmp.ResourcePrice <= this.resources && tmp.MoneyPrice <= this.money && this.Buildings.Count < this.buildingLimit)
                {

                    toBuild = tmp;

                }
                else
                {

                    Destroy(ObjectToBuild);
                    return;

                }

            }
            else
            {

                Destroy(ObjectToBuild);
                return;

            }

            toBuild.transform.position = new Vector3(0, -10, 0);

            currState = State.BuilderMode;

        }
        else if (Input.GetMouseButtonDown(1))
        {
            inputDelay = 0.2f;
            this.currState = State.SingleSelection;

        }


    }


    public void GameMenuShow()
    {

        this.UnsellectAll();
        this.toBuild = null;
        Destroy(ObjectToBuild);
        this.currState = State.NormalMode;
        this.GameMenu.SetActive(true);
        this.GameMenu.transform.Find("Panel").GetComponent<GameMenu>().Pause();

    }


}
