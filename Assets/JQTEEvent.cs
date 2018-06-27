using UnityEngine;
using System.Collections;

public interface JQTEEvent {
    void GetClick(int N);
    void Animate(float DT);
    void OnComplete();
}




public class JBaseQte: MonoBehaviour, JQTEEvent
{
    public enum Types{
    QTEBASE,
    QTEBUTTON,
    QTETIMEBUTTON,
    QTEBALLANCE,
    QTESLIDE,
    QTERUBBER
};
    public Types QTEType;
    public Vector3 MousePos;
    protected int ClickNum;
    public float totalTime;
    public float alpha;
    protected float BaseAlpha;
    protected float Speed;


    public JBaseQte()
    {
        alpha = 0;
        BaseAlpha = 0;
        Speed = 1.0f;
    }
    public void setBaseAndSpeed(float ba, float s)
    {
        BaseAlpha=ba;
        Speed=s;
    }
    public void GetClick(int N)
    {
        ClickNum = N;
        MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
    }
    public virtual void Animate(float DT)
    {
        //do nothing
    }
   

    public void OnComplete()
    {
        onCompDelegate(alpha, QTEType);
    }
    public delegate void MultiDelegate(float value,JBaseQte.Types T);
    public MultiDelegate onCompDelegate;


    void Update()
    {
        float DT = Time.deltaTime;
        this.Animate(DT);
        totalTime -= DT;
        if(totalTime<=0.0f||alpha>=1.0f)
        {
            OnComplete();
        }
        if(Input.anyKeyDown)
        {
            GetClick(69);
        }
    }
}