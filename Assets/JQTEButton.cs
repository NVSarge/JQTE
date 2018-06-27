using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class JQTEButton : JBaseQte
{
    public Image QTEFill;
    public Vector3 Pos;
    float DeltaFreq;
    float lastClick;

    public override void Animate(float DT)
    {
        
        alpha -= DT / 10.0f;
        if(alpha<0.0f)
        {
            alpha = 0.0f;
        }
        lastClick += DT;
        QTEFill.fillAmount = alpha;       
        if (ClickNum == 69)
        {
            ClickNum = 0;            
            if (lastClick < DeltaFreq)
            {                    
                    alpha += 0.04f;
            }                
            lastClick = 0.05f;
            
        }       
       
    }


    // Use this for initialization
    void Start()
    {
        QTEType = JBaseQte.Types.QTEBUTTON;
        DeltaFreq = 0.5f;
        lastClick = 0.0f;
        alpha = 0.6f;
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { GetClick(2); });
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
}
