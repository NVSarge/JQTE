using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class JDoll : MonoBehaviour {
    public GameObject SpecButton;
    public Image LeftHP;
    public Image RightHP;
    public Unit Player;
    public Unit Foe;

    public GameObject Player_Sprite;

    public Text Wins;

    public ParticleSystem PlayerBlood;
    public ParticleSystem FoeBlood;
    Vector3 OldMousePos;
    float PHp;
    float FHp;
    int LWins;
    int RWins;
    public Attacker Pl;
    public  Attacker Fe;
    Animator PlAnm;
    Animator FeAnm;
    public AutoShade ASh;
    bool isReset;
	// Use this for initialization
	void Start () {
        SpecButton.SetActive(true);
        OldMousePos = Player_Sprite.transform.position;        
        PlAnm = Pl.gameObject.GetComponent<Animator>();
        FeAnm = Fe.gameObject.GetComponent<Animator>();
        LWins = PlayerPrefs.GetInt("LW");
        RWins = PlayerPrefs.GetInt("RW");
        ASh.StartShade(GenUnits,1.0f,"");
      
	}
    
    void GenUnits()
    {
        Foe.GenRandom();
        Player.GenRandom();
        PHp = Player.Armor;
        FHp = Foe.Armor;
        Pl.isPlay = true;
        Fe.isPlay = true;
    }

    void ResetFunc()
    {
        PlayerPrefs.SetInt("LW", LWins);
        PlayerPrefs.SetInt("RW", RWins);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);      
    }
	

    public void DecrFoeHP(float DMG,bool isFoe)
    {
        if (!isFoe)
        {
            FoeBlood.Play();
            FHp -= Player.Att + DMG;
            if (FHp < .0f)
                FHp = 0.0f;
            if (FHp > 4.0f)
                FHp = 4.0f;
        }
        else
        {
            PlayerBlood.Play();
            PHp -= Foe.Att + DMG;
            if (PHp < .0f)
                PHp = 0.0f;
            if (PHp > 4.0f)
                PHp = 4.0f;
        }
        if (PHp <= 0)
        {
            RWins++;
            Pl.isPlay = false;
            Fe.isPlay = false;
            ResetFunc();
        }
        else if (FHp <= 0)
        {
            LWins++;
            Pl.isPlay = false;
            Fe.isPlay = false;
           ResetFunc();
        }       

    }
    
    public void DragDoll()
    {      
      /*  Vector3 scCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        scCoord.z = 0;
        Player_Sprite.transform.position = scCoord+Vector3.up*2.0f;/**/
    }


    public void PlayAttack()
    {
        PlAnm.SetTrigger("tAttack");
    }

    public void PlayAttackOff()
    {
        PlAnm.SetTrigger("tAttackOff");
    }


    public void UseSpec()
    {
        PHp++;
        SpecButton.SetActive(false);
    }
   
	// Update is called once per frame
	void Update () {
        if (Player.isModed || Foe.isModed)
        {
            Debug.Log(123);
            Foe.isModed = false;
            Player.isModed = false;
            PHp = Player.Armor;
            FHp = Foe.Armor;
        }
        LeftHP.fillAmount = PHp / 4.0f;
        RightHP.fillAmount = FHp / 4.0f;
        Wins.text = LWins + " : " + RWins;
        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.SetInt("LW", 0);
            PlayerPrefs.SetInt("RW", 0);
            Application.LoadLevel(0);

        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
