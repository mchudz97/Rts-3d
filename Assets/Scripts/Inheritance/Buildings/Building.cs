using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Building : MonoBehaviour, IBuilding, ISelectable
{
    public enum TYPE { NOONE=0,BASE, FACTORY, DEFENSIVE, RESOURCES}
    public float CurrHp;
    public float MaxHp;
    public int MoneyPrice;
    public int ResourcePrice;
    public bool IsBuilded;
    public virtual GameObject Colldetector { get; set; }
    public TYPE BuildingType;
    public virtual HealthBar HealthBar { get; set; }
    public virtual GameObject SelectBox { get; set; }
    
    public abstract void ToUpdate();

    private NavMeshObstacle NavMeshObstacle;

    public bool TryToBuild(Vector3 origin, bool clicked)
    {
        

        this.transform.position = origin;

        if (IsSurfaceAvailable())
        {

            

            if (clicked)
            {
                
                Build(origin);
                return true;
            }

        }

        return false;

    }

    public void Build(Vector3 origin)
    {

        Debug.Log("kolizje: "+ Colldetector.GetComponent<CapArea>().collisions.Count + Colldetector.transform.position + this.transform.position);

        this.transform.position = origin;
        Destroy(Colldetector);
        this.NavMeshObstacle.enabled = true;
        IsBuilded = true;

    }

   

    public bool IsSurfaceAvailable()
    {
        
        return this.Colldetector.GetComponent<CapArea>().CheckIsAvailable();

    }

   
   
    public void TakeDamageFromUnit(GameObject unit)
    {

        MyUnit unitScript;

        if((unitScript = unit.GetComponent<MyUnit>()) != null)
        {
         
            this.CurrHp -= unitScript.Damage;


            if(this.CurrHp <= 0)
            {

                Destroy(this.transform.gameObject);
                return;

            }

        }
        

    }

    public void TakeDamageFromTower(GameObject building)
    {
        Tower towerScript;
        
        if ((towerScript = building.GetComponent<Tower>()) != null)
        {

            this.CurrHp -= towerScript.Damage;

            if (this.CurrHp <= 0)
            {

                Destroy(this.transform.gameObject);
                return;

            }

        }
    }
    
    
    
    void Start()
    {

        this.IsBuilded = false;
        this.NavMeshObstacle = this.transform.gameObject.GetComponent<NavMeshObstacle>();
        this.Colldetector = this.transform.Find("CapableArea").gameObject;
        this.HealthBar = this.transform.Find("HealthBar").GetComponent<HealthBar>();
        this.SelectBox = this.transform.Find("SelectBox").gameObject;

        this.NavMeshObstacle.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        this.HealthBar.UpdateBar(this.CurrHp / this.MaxHp);
        ToUpdate();

    }

    public void Select()
    {

        this.SelectBox.SetActive(true);
    
    }

    public void Unselect()
    {
        this.SelectBox.SetActive(false);
    }

}
