using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class JQTEBallance : JBaseQte
{
    public Image pointer;
    public Image sl;
    public Vector3 Pos;
    float deltaalpha;
    int bounces;

    public override void Animate(float DT)
    {
        float add=DT * deltaalpha*2.0f;
        if (ClickNum == 69 || bounces >= 10)
        {
            ClickNum = 0;
            OnComplete();

        }
        else
        {
            pointer.transform.position = new Vector3((2.0f * alpha - 1.0f) * 1.2f, -0.3f, 0);
            if (alpha + add >= 1.0f)
            {
                bounces++;
                alpha = 0.999f;
                deltaalpha = -1.0f;
            }
            else if (alpha + add <= 0.0f)
            {
                bounces++;
                alpha = 0.001f;
                deltaalpha = 1.0f;
            }
            else
            {
                alpha += add;
            }
        }
        
     

    }


    // Use this for initialization
    void Start()
    {
        QTEType = JBaseQte.Types.QTEBALLANCE;
        deltaalpha = Random.Range(0,2)*2-1.0f;
        float Width=(BaseAlpha/Speed)*0.5f;
        sl.transform.localScale=new Vector3(Width,1,1);
        alpha = 1.0f-deltaalpha;
        totalTime = 100;
        bounces=0;
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { GetClick(2); });
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
}
