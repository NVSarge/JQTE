using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class JQTERubber: JBaseQte
{
    public Image s1;
    public Image s2;
    public Vector3 Pos;
    public float Rad;
    

    public override void Animate(float DT)
    {
        Debug.Log(totalTime);
       
        if(ClickNum==2)
        {
            s1.transform.position = Pos;
            s2.transform.position = Pos;
            totalTime = 0.3f;
            ClickNum = 0;
        }
        if (ClickNum == 1)
        {

            alpha = (MousePos - Pos).magnitude;
            s1.transform.position = (MousePos - Pos) * (1.0f/alpha) + Pos;
            s2.transform.position = Pos;
        }
        
    }


    // Use this for initialization
    void Start()
    {
        QTEType = JBaseQte.Types.QTERUBBER;
        s1 = GetComponent<Image>();
        Pos = s1.transform.position;        
        alpha = 0.0f;
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { GetClick(1); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.EndDrag;
        entry2.callback.AddListener((data) => { GetClick(2); });
        trigger.triggers.Add(entry2);
    }

    // Update is called once per frame
}
