using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{

    public int Damage;
    public int Range;
    public float ReloadTime;
    public float Reloading;
    public GameObject Target { get; set; }



    // Start is called before the first frame update
    public LineRenderer lineRenderer;

    IEnumerator enumerator()
    {

        yield return new WaitForSeconds(.1f);

        this.lineRenderer.enabled = false;

    }

    public void FindTarget()
    {

        if (this.CompareTag("Untagged"))
        {

            return;

        }

        var enemyUnits = "EnemyUnits";
        var mineUnits = "MineUnits";

        if (!this.transform.gameObject.CompareTag(mineUnits))
        {
            
            enemyUnits = mineUnits;

        }

        if (this.Target == null || Vector3.Distance(this.transform.position, Target.transform.position) > this.Range)
        {
            this.Target = null;
            GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(enemyUnits);

            foreach( GameObject pT in possibleTargets)
            {

                if (Vector3.Distance(pT.transform.position, this.transform.position) <= this.Range)
                {

                    this.Target = pT;
                    break;

                }

            }

        }
                
    }

    public void Attack()
    {

        this.FindTarget();
        if(this.Target != null)
        {
            
            MyUnit targetUnit;
            Building targetBuilding;
            if((targetUnit = Target.GetComponent<MyUnit>()) != null && Reloading >= ReloadTime)
            {
                this.lineRenderer.enabled = true;
                Vector3 vec1 = this.transform.position + new Vector3(0, 3, 0);
                Vector3 vec2 = Target.transform.position + new Vector3(0, 1, 0);
                this.lineRenderer.SetPosition(0, vec1);
                this.lineRenderer.SetPosition(1, vec2);
                targetUnit.TakeDamageFromTower(this.transform.gameObject);
                Reloading = 0;
                StartCoroutine("enumerator");

            }
            else if((targetBuilding = Target.GetComponent<Building>()) != null && Reloading >= ReloadTime)
            {

                this.lineRenderer.enabled = true;
                Vector3 vec1 = this.transform.position + new Vector3(0, 3, 0);
                Vector3 vec2 = Target.transform.position + new Vector3(0, 3, 0);
                this.lineRenderer.SetPosition(0, vec1);
                this.lineRenderer.SetPosition(1, vec2);
                targetBuilding.TakeDamageFromTower(this.transform.gameObject);
                Reloading = 0;
                StartCoroutine("enumerator");


            }

        }


    }

    public override void ToUpdate()
    {
        Attack();
        Reloading += Time.deltaTime;

    }
}
