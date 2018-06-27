using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class Programm
{


    public static void Shuffle<T>(List<T> array)
    {
        int n = array.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(Random.value * (n - i));
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

}
public class JQTE : MonoBehaviour
{
    public GameObject SpecButton;
    public Canvas Cnvs;
    public Text Res;
    public GameObject[] qtPref;
    // Use this for initialization
    public List<JBaseQte> jqb;
    public Animator LeftAnimator;
    public Animator RightAnimator;
    public Image LeftHP;
    public Image RightHP;
    int PHp;
    int FHp;
    public Unit Player;
    public Unit Foe;
    public AutoShade ASh;

    int LWins;
    int RWins;
    int AttTurns;
    int CAttTurn;
    Dictionary<JBaseQte.Types, int> NUms;
    JBaseQte current;
    bool isReload;
    void Start()
    {
        SpecButton.SetActive(true);
        isReload = false;
        PHp = Player.Armor;
        FHp = Foe.Armor;
        AttTurns = 5;
        /*  List<int> NUms = new List<int>();
          for (int j = 0; j < qtPref.Length;j++ )
          {
              NUms.Add(j);
              NUms.Add(j);
              NUms.Add(j);
              NUms.Add(j);
          }        
          Programm.Shuffle(NUms);*/

        NUms = new Dictionary<JBaseQte.Types, int>();
        //????
        NUms.Add(JBaseQte.Types.QTEBUTTON, 0);
        NUms.Add(JBaseQte.Types.QTETIMEBUTTON, 1);
        NUms.Add(JBaseQte.Types.QTEBALLANCE, 2);
        NUms.Add(JBaseQte.Types.QTESLIDE, 3);
        NUms.Add(JBaseQte.Types.QTERUBBER, 4);
        ////

        LWins = PlayerPrefs.GetInt("LW");
        RWins = PlayerPrefs.GetInt("RW");
        ASh.StartShade(GenUnits, 1.0f,"");
    }


    void GenUnits()
    {
        Foe.GenRandom();
        Player.GenRandom();
        PHp = Player.Armor;
        FHp = Foe.Armor;
        StopAllCoroutines();
        AddQTEObject(JBaseQte.Types.QTEBALLANCE, false, 0.3f);
    }

    void ResetFunc()
    {
        PlayerPrefs.SetInt("LW", LWins);
        PlayerPrefs.SetInt("RW", RWins);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    void AddQTEObject(JBaseQte.Types T,bool isR, float fPause)
    {
        if (current != null)
        {
            DestroyImmediate(current.gameObject);
        }
        StartCoroutine(AddQte(T, isR, fPause));        
    }

    public IEnumerator AddQte(JBaseQte.Types T,bool isRandom, float fPause)
    {

        Vector3 R = Random.onUnitSphere;
        R.z = 0;
        R.y *= 1.0f;
        yield return new WaitForSeconds(fPause);
        if (NUms.ContainsKey(T) && current == null)
        {
            GameObject go=null;
            if(isRandom)
            {   
                go = Instantiate(qtPref[NUms[T]],R,Quaternion.identity) as GameObject;
            }
            else
            {
                go = Instantiate(qtPref[NUms[T]]) as GameObject;
            }
            
            go.transform.SetParent(Cnvs.transform, false);
            if (isRandom)
            {
                go.transform.Translate(R);
            }
            current = go.GetComponent<JBaseQte>();           
            current.onCompDelegate += OnComplete;
            current.setBaseAndSpeed(Player.Dext, Foe.Dext);
        }

    }

    void OnComplete(float value, JBaseQte.Types t)
    {
        Debug.Log("V:"+value);
        bool isRandom = false;
        float fP = 0.5f;
        JBaseQte.Types TypeToCreate=JBaseQte.Types.QTEBASE;
        switch (t)
        {
            case JBaseQte.Types.QTEBALLANCE:
                float Width = ((float)Player.Dext / (float)Foe.Dext)*0.5f;
                Debug.Log((1.0f - Width) * 0.5f);
                if (value <= (1.0f - Width) * 0.5f || value > (1.0f + Width) * 0.5f)                    
                {
                   RightAnimator.SetTrigger("tAttack");
                   TypeToCreate=JBaseQte.Types.QTEBUTTON;                    
                }
                else                
                {
                    
                   LeftAnimator.SetTrigger("tAttack");
                   CAttTurn = AttTurns;
                   TypeToCreate=JBaseQte.Types.QTETIMEBUTTON;
                   isRandom = true;
                }
                break;
            case JBaseQte.Types.QTETIMEBUTTON:
                if(value>0.1f)
                {
                    CAttTurn--;
                    if (CAttTurn <= 0)
                    {
                        FHp-=Player.Att;
                        RightAnimator.SetTrigger("tWound");
                        LeftAnimator.SetTrigger("tAttackOff");
                        TypeToCreate = JBaseQte.Types.QTEBALLANCE;
                    }
                    else
                    {
                        isRandom = true;
                        TypeToCreate = JBaseQte.Types.QTETIMEBUTTON;
                        fP = 0.1f;
                    }
                }
                else
                {
                    RightAnimator.SetTrigger("tBlock");
                    LeftAnimator.SetTrigger("tAttackOff");
                    TypeToCreate = JBaseQte.Types.QTEBALLANCE;
                }
                
                break;
            case JBaseQte.Types.QTEBUTTON:
                if (value <0.9f)
                {
                    PHp-=Foe.Att;
                    LeftAnimator.SetTrigger("tWound");
                }
                else
                {                  
                    LeftAnimator.SetTrigger("tBlock");
                }
                RightAnimator.SetTrigger("tAttackOff");
                TypeToCreate = JBaseQte.Types.QTEBALLANCE;
                break;
        }

        if ((PHp <= 0 || FHp <= 0))
        {
            if (PHp <= 0)
            {
                RWins++;
               
            }
            else
            {
                LWins++;
               
            }
            if (current != null)
            {
                DestroyImmediate(current.gameObject);
            }
            ResetFunc();
        }
        else
        {
            AddQTEObject(TypeToCreate, isRandom, fP);
        }

    }
    public void UseSpec()
    {
        PHp++;
        SpecButton.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Player.isModed || Foe.isModed)
        {
            Foe.isModed = false;
            Player.isModed = false;
            PHp = Player.Armor;
            FHp = Foe.Armor;
        }
        Res.text = LWins + " : " + RWins;
        LeftHP.fillAmount = PHp / 4.0f;
        RightHP.fillAmount = FHp / 4.0f;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Menu)||Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.SetInt("LW", 0);
            PlayerPrefs.SetInt("RW", 0);
            Application.LoadLevel(1);
        }
        
    }
}
