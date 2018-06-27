using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class JQTESlide : JBaseQte
{
    public Slider sl;
    public Vector3 Pos;
    public Vector3 SPos;
    float Length;

    public override void Animate(float DT)
    {
        if (ClickNum == 1)
        {
            SPos = Pos;
        }
        if (ClickNum == 2)
        {
            alpha += (MousePos - SPos).magnitude*5.0f / Length;
            ClickNum = 0;

        }
        sl.value = alpha;

    }


    // Use this for initialization
    void Start()
    {
        QTEType = JBaseQte.Types.QTESLIDE;
        sl = GetComponent<Slider>();
        Length = sl.fillRect.rect.size.x;
        if (Length == 0.0f)
        {
            Length = 1.0f;
        }
        Debug.Log(Length);
        alpha = 0.0f;
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((data) => { GetClick(1); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.EndDrag;
        entry2.callback.AddListener((data) => { GetClick(2); });
        trigger.triggers.Add(entry2);
    }

    // Update is called once per frame
}
