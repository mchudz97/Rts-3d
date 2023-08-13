using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{

    public RectTransform Box { get; set; }
    public Vector2 StartPos { get; set; }
    public Texture xD;
    // Start is called before the first frame update
    void Start()
    {

        this.Box = this.transform.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StretchTo(Vector2 pos)
    {

        var width = pos.x - StartPos.x;
        var height = pos.y - StartPos.y;

        

        this.Box.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        this.Box.anchoredPosition = (Vector2)StartPos + new Vector2(width/2, height/2);


    }
   

}
