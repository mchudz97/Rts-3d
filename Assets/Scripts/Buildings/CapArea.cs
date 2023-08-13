using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapArea : MonoBehaviour
{

    public List<GameObject> collisions = new List<GameObject>(0);

    bool isAvailable = true;

    bool isGreen = true;
    

    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {

        renderer = this.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {

        collisions.Add(collision.gameObject);


    }

    private void OnCollisionExit(Collision collision)
    {

        collisions.Remove(collision.gameObject);

    }
  

    public bool CheckIsAvailable()
    {

        
        foreach (GameObject g in collisions)
        {
            

            if (g.transform.parent != this.transform.parent)
            {

                if (isGreen) {

                    renderer.material.SetColor("_Color", Color.red);
                    isGreen = false;

                }

                return false;

            }

        }

        

        if (!isGreen)
        {

            renderer.material.SetColor("_Color", Color.green);
            isGreen = true;

        }

        return true;

    }


}
