using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    float screenBorderDetect = 20f;
   
    float camSpeed = 20f;
    float rotSpeed = 40f;
   
    

    float zoomSpeed = 10;
    float minH = 5;
    float maxH = 20;

    void Start()
    {

        

    }

    void Update()
    {

        

    }

    private void FixedUpdate()
    {

        MoveCam();
        RotateCam();

    
    }

    void MoveCam()
    {
        
        float camPosX = Camera.main.transform.position.x;
        float camPosZ = Camera.main.transform.position.z;
        float camPosY = Camera.main.transform.position.y;

        Transform camPos = Camera.main.transform;

        float mousePosX = Input.mousePosition.x;
        float mousePosY = Input.mousePosition.y;
        
        if (mousePosX <= screenBorderDetect && mousePosX > 0)
        {
           
            camPos.position -= camPos.right * camSpeed * (screenBorderDetect - mousePosX) * 3 / screenBorderDetect  * Time.deltaTime;
           
        }

        if(mousePosX >= ( Screen.width - screenBorderDetect ) && Screen.width > mousePosX)
        {

            camPos.position += camPos.right * camSpeed * (mousePosX - (Screen.width - screenBorderDetect)) * 3 / screenBorderDetect * Time.deltaTime;

        }

        if (mousePosY <= screenBorderDetect && mousePosY > 0)
        {
            
            camPos.position -= camPos.forward * camSpeed * (screenBorderDetect - mousePosY) * 3 / screenBorderDetect  * Time.deltaTime;
 
        }

        if(mousePosY >= (Screen.height - screenBorderDetect) && Screen.height > mousePosY)
        {

            camPos.position += camPos.forward * camSpeed * (mousePosY - (Screen.height - screenBorderDetect)) * 3 /screenBorderDetect * Time.deltaTime;

        }

         camPosY -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
         camPosY = Mathf.Clamp(camPosY, minH, maxH);
         camPos.position = new Vector3(camPos.position.x, camPosY, camPos.position.z);
        

    }

    void RotateCam()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;


        if (Input.GetKey(KeyCode.R))
        {

            origin += Vector3.up * Time.deltaTime * rotSpeed;

        }
        if (Input.GetKey(KeyCode.E))
        {

            origin -= Vector3.up * Time.deltaTime * rotSpeed;

        }

        Camera.main.transform.eulerAngles = origin;
    }

    public Vector3? GetMapCordsFromMouse()
    {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(mouseRay, out hit, 50, LayerMask.GetMask("SUPERLEJER")))
        {
 

            if (hit.collider.tag == "Ground")
            {
                
                return hit.point;

            }

        }

        return null;

    }

    public GameObject SingleTarget()
    {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, 50))
        {
            

            if (hit.collider.gameObject.transform.parent.CompareTag("MineUnits"))
            {

                return hit.collider.gameObject.transform.parent.gameObject;

            }

        }

        return null;

    }


    public Vector3? GetCord()
    {


        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit))
        {


            if (hit.collider != null)
            {

                return hit.point;

            }

        }

        return null;


    }

    public GameObject getEnemy(Vector2 at)
    {

        Ray vecRay = Camera.main.ScreenPointToRay(at);
        RaycastHit hit;

        if (Physics.Raycast(vecRay, out hit, 500))
        {

            if (hit.collider.gameObject.transform.parent.CompareTag("EnemyUnits"))
            {

                return hit.collider.gameObject.transform.parent.gameObject;

            }

        }

        return null;

    }


}
    







