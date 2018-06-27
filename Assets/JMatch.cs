using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class JActor
{
    public JAct JA;
    public GameObject go;
}

public class JMatch : MonoBehaviour {
    public GameObject SpecButton;
    public Transform Cnvs;
    public Text Res;
    public Image PBarFoe;
    public Image LeftHP;
    public Image RightHP;
    public GameObject[] IcosPref;
    public JAct[] Jacts;
    public Unit Player;
    public Unit Foe;
    public AutoShade ASh;
    float PHp;
    float FHp;
    int LWins;
    int RWins;
    List<JActor> IcosPanel;
    List<JActor> IcosStack;
    List<GameObject> StackGO;
	// Use this for initialization
	void Start () {
        IcosPanel = new List<JActor>();
        IcosStack = new List<JActor>();
        StackGO = new List<GameObject>();                
        LWins = PlayerPrefs.GetInt("LW");
        RWins = PlayerPrefs.GetInt("RW");
        PHp = Player.Armor;
        FHp = Foe.Armor;
        ASh.StartShade(GenUnits, 1.0f, "");

	}

    void GenUnits()
    {
        Foe.GenRandom();
        Player.GenRandom();
        PHp = Player.Armor;
        FHp = Foe.Armor;
        StopAllCoroutines();
        StackGO.ForEach(child => DestroyImmediate(child));
        IcosPanel.Clear();
        IcosStack.Clear();
        GenField();
        StartCoroutine(FoeMove());
    }

    void ResetFunc()
    {
        PlayerPrefs.SetInt("LW", LWins);
        PlayerPrefs.SetInt("RW", RWins);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator Battle()
    {
        SpecButton.SetActive(true);
        float BattleSpeed = 0.4f;
        while(IcosStack.Count>0)
        {
            float DT = 0;
            while (DT < BattleSpeed)
            {
                DT += Time.deltaTime;
                PBarFoe.fillAmount = DT / BattleSpeed;
                yield return new WaitForSecondsRealtime(0.01f);
            }
                JActor ja= IcosStack[IcosStack.Count-1];
                IcosStack.RemoveAt(IcosStack.Count - 1);            
                ///shity shit
                if(ja.go.transform.position.x>0)
                {
                    if(ja.JA.T.Equals(JAct.Types.QAtt))
                    {
                        PHp-= 1.0f;
                    }
                    else
                    {
                        if (FHp < 4.0f)
                        {
                            FHp += 1.0f;
                        }
                    }
                }else
                {
                    if (ja.JA.T.Equals(JAct.Types.QAtt))
                    {
                        FHp -= 1.0f;
                    }
                    else
                    {
                        if (PHp < 4.0f)
                        {
                            PHp += 1.0f;
                        }
                    }
                }
                ja.go.SetActive(false);
                ///              
                if (PHp <= 0 || FHp <= 0)
                {
                    if (PHp <= 0)
                    {
                        RWins++;
                        
                    }
                    else
                    {
                        LWins++;
                       
                    }

                    ResetFunc();
                }
        }
        if(LeftHP.fillAmount>0&&RightHP.fillAmount>0)
        {
            IcosPanel = new List<JActor>();
            IcosStack = new List<JActor>();
            GenField();
            StartCoroutine(FoeMove());
        }
    }

    public  IEnumerator FoeMove()
    {
        float DT = 0;        
        
        while(DT<5.0f)
        {
            DT += Time.deltaTime * (float)Foe.Dext*3.0f;
            PBarFoe.fillAmount =  DT/ 5.0f;
            yield return new WaitForSecondsRealtime(0.01f);    
        }
        if (IcosPanel.Count > 0)
        {
            JActor ja= IcosPanel[0];
            IcosPanel.RemoveAt(0);
            ja.go.transform.position = new Vector3(0.5f, (4.0f-IcosStack.Count)*0.5f, 0);
            ja.go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            IcosStack.Add(ja);
            Destroy(ja.go.GetComponent<Button>());
            StartCoroutine(FoeMove());
        }
        else
        {
             StartCoroutine(Battle());
        }
    }

    void GenField()
    {
        StackGO.ForEach(child => DestroyImmediate(child));
        for (int i = 0; i < Player.Att + Foe.Att; i++)
        {
            int L = IcosPanel.Count;
            Vector3 R = new Vector3(L%5-2,L/5-4,0);// Random.onUnitSphere;                     
            GameObject go = Instantiate(IcosPref[0],R,Quaternion.identity) as GameObject;
            Button btn=go.AddComponent<Button>();
            JActor ja = new JActor();
            ja.go = go;
            ja.JA = Jacts[0];
            btn.onClick.AddListener(delegate { Add(btn); });
            go.transform.SetParent(Cnvs.transform, false);
            go.transform.Translate(R);            
            IcosPanel.Add(ja);
            StackGO.Add(go);
        }
        for (int i = 0; i < Player.Armor + Foe.Armor; i++)
        {
            int L = IcosPanel.Count;
            Vector3 R = new Vector3(L % 5 - 2, L / 5 - 4, 0);// Random.onUnitSphere;                     
            GameObject go = Instantiate(IcosPref[1], R, Quaternion.identity) as GameObject;
            Button btn = go.AddComponent<Button>();
            JActor ja = new JActor();
            ja.go = go;
            ja.JA = Jacts[1];
            btn.onClick.AddListener(delegate { Add(btn); });
            go.transform.SetParent(Cnvs.transform, false);
            go.transform.Translate(R);
            IcosPanel.Add(ja);
            StackGO.Add(go);
        }
    }

    public void Add(Button b)
    {
        int n = 0;
        JActor ja=IcosPanel.Find(x=>x.go.Equals(b.gameObject));
       if(ja!=null)         
       {
           IcosPanel.Remove(ja);
           Destroy(ja.go.GetComponent<Button>());
           ja.go.transform.position = new Vector3(-0.5f, (4.0f - IcosStack.Count) * 0.5f, 0);
           ja.go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
           IcosStack.Add(ja);
       }
           
        

    }
    public void UseSpec()
    {
        PHp++;
        SpecButton.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        if(Player.isModed||Foe.isModed)
        {
            Foe.isModed = false;
            Player.isModed = false;
            PHp = Player.Armor;
            FHp = Foe.Armor;
        }
        Res.text = LWins + " : " + RWins;
        LeftHP.fillAmount = PHp / 4.0f;
        RightHP.fillAmount = FHp / 4.0f;
        if (Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.SetInt("LW", 0);
            PlayerPrefs.SetInt("RW", 0);
            Application.LoadLevel(2);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
