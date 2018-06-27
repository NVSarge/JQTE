using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAlarm : MonoBehaviour {
    public JDoll JDOllRef;
    bool isAttacked;
    public bool isFoe;
	// Use this for initialization
	void Start () {
        isAttacked = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (JDOllRef!=null)
        {
            if (coll.gameObject.tag.Equals("target") && !isAttacked)
            {
                JDOllRef.DecrFoeHP(0, isFoe);
               // JDOllRef.ReturnDoll();
                isAttacked = true;
            }
            if (coll.gameObject.tag.Equals("block"))
            {
               // JDOllRef.ReturnDoll();
            }
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
     
            if (coll.gameObject.tag.Equals("target"))
            {
                isAttacked = false;
            }
     
    }
}
