using Assets.Scripts.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MyUnit : MonoBehaviour, IUnit, ISelectable
{
    public enum TYPE { SOLDIER, TANK}
    public TYPE UnitType;
    public int Health;
    public int MaxHealth;
    public int Damage;
    public GameObject Target;
    public float ReloadTime;
    public float Reloading;
    public int AttackRange;
    public int MoneyCost;
    public int ResourceCost;
    public NavMeshAgent Agent;
    public GameObject SelectBox;
    private Vector3 prevPos;
    private float checkIsMoving;
    private LineRenderer lineRenderer;
    public virtual HealthBar HealthBar { get; set; }



    IEnumerator enumerator()
    {

        yield return new WaitForSeconds(.1f);

        this.lineRenderer.enabled = false;

    }

    public void Attack(GameObject target)
    {

        if (!this.Agent.isActiveAndEnabled)
        {

            return;

        }


        if(target == null)
        {
            target = null;
            this.Agent.isStopped = false;
            return;

        }
        if (this.CompareTag("Untagged"))
        {

            return;

        }
        

        if(Vector3.Distance(this.transform.position, target.transform.position) > this.AttackRange)
        {
            
            this.MoveTo(target.transform.position);
            this.Target = target;
        }
        else
        {

            this.Agent.isStopped = true;
            this.transform.LookAt(target.transform);
            if (Reloading >= ReloadTime)
            {
                

                MyUnit tarUnitScript;
                Building tarBuildScript;

                if ((tarUnitScript = target.GetComponent<MyUnit>()) != null)
                {

                    this.lineRenderer.enabled = true;
                    Vector3 vec1 = this.transform.position + new Vector3(0, 1, 0);
                    Vector3 vec2 = target.transform.position + new Vector3(0, 1, 0);
                    this.lineRenderer.SetPosition(0, vec1);
                    this.lineRenderer.SetPosition(1, vec2);
                    tarUnitScript.TakeDamageFromUnit(this.transform.gameObject);
                    this.Reloading = 0;
                    StartCoroutine("enumerator");

                }
                else if((tarBuildScript = target.GetComponent<Building>()) != null)
                {

                    this.lineRenderer.enabled = true;
                    Vector3 vec1 = this.transform.position + new Vector3(0, 1, 0);
                    Vector3 vec2 = target.transform.position + new Vector3(0, 1, 0);
                    this.lineRenderer.SetPosition(0, vec1);
                    this.lineRenderer.SetPosition(1, vec2);
                    tarBuildScript.TakeDamageFromUnit(this.transform.gameObject);
                    this.Reloading = 0;
                    StartCoroutine("enumerator");

                }


            }

        }

        
    }


    public void TakeDamageFromUnit(GameObject from)
    {

        MyUnit fromUnitScript;

        if((fromUnitScript = from.GetComponent<MyUnit>()) != null)
        {

            this.Health -= fromUnitScript.Damage;
            if (this.Health <= 0)
            {

                Destroy(this.transform.gameObject);
                return;

            }
            if (this.Agent.isStopped && this.Target == null || this.transform.CompareTag("EnemyUnits"))
            {

                this.Target = from;
                Attack(from);

            }


        }
       
    }

    public void TakeDamageFromTower(GameObject from)
    {

        Tower fromTowerScript;

        if ((fromTowerScript = from.GetComponent<Tower>()) != null)
        {

            this.Health -= fromTowerScript.Damage;

        }

        if (this.Health <= 0)
        {

            Destroy(this.transform.gameObject);
            return;

        }

        if (this.Agent.isStopped && this.Target == null || this.transform.CompareTag("EnemyUnits"))
        {
            this.Target = from;
            Attack(from);

        }


    }

    public void FreeAttack()
    {
        if(this.Target == null)
        {

            string enemyTag = "EnemyUnits";
            string friendlyTag = "MineUnits";

            if (this.transform.gameObject.CompareTag(enemyTag))
            {

                enemyTag = friendlyTag;

            }

            foreach(GameObject e in GameObject.FindGameObjectsWithTag(enemyTag))
            {

                if(Vector3.Distance(this.transform.position, e.transform.position) <= this.AttackRange)
                {

                    Attack(e);
                    return;

                }


            }


        }

    }

   

    public void MoveTo(Vector3 place)
    {

        if (this.Agent.isActiveAndEnabled)
        {

            this.Target = null;
            this.Agent.isStopped = false;
            this.Agent.SetDestination(place);

        }
      

    }

    // Start is called before the first frame update
    void Start()
    {
        this.lineRenderer = this.transform.Find("GUN").GetComponent<LineRenderer>();
        this.SelectBox = this.transform.Find("SelectBox").gameObject;
        this.HealthBar = this.transform.Find("HealthBar").GetComponent<HealthBar>();

    }

    // Update is called once per frame
    void Update()
    {
        this.FreeAttack();
        this.HealthBar.UpdateBar((float)this.Health / this.MaxHealth);
        this.Attack(this.Target);
        this.Reloading += Time.deltaTime;

        if (this.Agent.isActiveAndEnabled && !this.Agent.isStopped)
        {
            this.checkIsMoving += Time.deltaTime;
            if(this.checkIsMoving >= 2f)
            {

                if(this.transform.position == this.prevPos)
                {
                    
                    this.Agent.isStopped = true;

                }

                this.checkIsMoving = 0;

            }

        }
        else
        {
            
            this.checkIsMoving = 0;

        }

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
