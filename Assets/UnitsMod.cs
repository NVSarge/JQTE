using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitsMod : MonoBehaviour {
    public Slider PlayerAt;
    public Slider PlayerDf;
    public Slider PlayerDx;

    public Slider FoeAt;
    public Slider FoeDf;
    public Slider FoeDx;
    public Unit Foe;
    public Unit Player;
    public Text UInfo;
    bool isSetup = false;
	// Use this for initialization
	void Start () {
        FoeAt.value = Foe.Att;
        FoeDx.value = Foe.Dext;
        FoeDf.value = Foe.Armor;

        PlayerAt.value = Player.Att;
        PlayerDx.value = Player.Dext;
        PlayerDf.value = Player.Armor;
        isSetup = true;
	}


    public void Reset()
    {
        isSetup = false;
        FoeAt.value = Foe.Att;
        FoeDx.value = Foe.Dext;
        FoeDf.value = Foe.Armor;

        PlayerAt.value = Player.Att;
        PlayerDx.value = Player.Dext;
        PlayerDf.value = Player.Armor;
        isSetup = true;
    }

    public void Apt()
    {
        if (isSetup)
        {
            Foe.Att = (int)FoeAt.value;
            Foe.Dext = (int)FoeDx.value;
            Foe.Armor = (int)FoeDf.value;

            Player.Att = (int)PlayerAt.value;
            Player.Dext = (int)PlayerDx.value;
            Player.Armor = (int)PlayerDf.value;
            Foe.isModed = true;
            Player.isModed = true;
        }
        UInfo.text = "P.Att: " + Player.Att + "  F.Att: " + Foe.Att + "\nP.Def: " + Player.Armor + "  F.Def: " + Foe.Armor + "\nP.Dx: " + Player.Dext + "  F.Dx: " + Foe.Dext;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
