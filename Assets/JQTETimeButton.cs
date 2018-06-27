using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class JQTETimeButton : JBaseQte
{
    public Image QTEFill;
    public Vector3 Pos;
   

    public override void Animate(float DT)
    {
        alpha -= DT/totalTime;
        float sc=alpha * 3.0f;
        QTEFill.rectTransform.localScale = new Vector3(sc, sc,sc);
        if (alpha < 0.0f)
        {
            alpha = 0.0f;
        }       
        if (ClickNum == 2)
        {
           ClickNum = 0;
           OnComplete();                    
        }               
    }

    
    // Use this for initialization
    void Start()
    {
        QTEType = JBaseQte.Types.QTETIMEBUTTON;
        alpha = 1.0f;
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { GetClick(2); });
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
}
