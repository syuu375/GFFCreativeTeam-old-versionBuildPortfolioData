using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SkillType
{
    ATTACK1,ATTACK2,ATTACK3,
    HP1, HP2, HP3,
    GUN1, GUN2, GUN3,
    SPEED1, SPEED2, SPEED3,
    UNB1, UNB2, UNB3,
    SKILL1, SKILL2, SKILL3,
    KNOCKS1, KNOCKS2, KNOCKS3,
    KNOCKP1, KNOCKP2, KNOCKP3,
    SLASH,GROUND,SHOTGUN,RIFLE
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    private TextMeshProUGUI skillPointText;
    private TextMeshProUGUI skillnameText;
    private TextMeshProUGUI skillInfoText;
    private TextMeshProUGUI pointText;
    private GameObject skillBlockPanel;
    public int skillPoint;

    [SerializeField] List<SkillType> skillList = new List<SkillType>();//購入済みを把握するlist
    SkillBlock[] skillBlocks;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;   //一応
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        SceneManager.sceneLoaded += SceneLoaded;

    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)//シーンがロードされると呼ばれる
    {
        skillPointText = GameObject.Find("SkillPointText")?.GetComponent<TextMeshProUGUI>();
        skillnameText = GameObject.Find("SkillnameText")?.GetComponent<TextMeshProUGUI>();
        skillInfoText = GameObject.Find("SkillinfoText")?.GetComponent<TextMeshProUGUI>();
        pointText = GameObject.Find("PointText")?.GetComponent<TextMeshProUGUI>();
        skillBlockPanel = GameObject.Find("SkillBlock");

        
        if(skillBlockPanel != null)
        {
            skillBlocks = skillBlockPanel.GetComponentsInChildren<SkillBlock>();

            foreach (var block in skillBlocks)
            {
                if (HasSkill(block.SkillType)) // ← SkillBlock に SkillType の public getter を作っておく
                {
                    block.SetLearnedColor();
                }
                else
                {
                    block.CheckActiveBlock();
                }
            }
        }
        if (skillBlockPanel == null) Debug.Log("skillBlockPanel null");
        if (skillPointText == null) Debug.Log("pointtext null");
        if (skillnameText == null) Debug.Log("nametext null");
        if (skillInfoText == null) Debug.Log("infotext null");
        if (pointText == null) Debug.Log("infotext null");
        if (skillBlockPanel == null) Debug.Log("Blockpanel null");

        
    }

    private void Start()
    {
        if (skillInfoText != null) UpdateSkillInfoText();
        if (skillnameText != null) UpdateSkillnameText();
        if (pointText != null) UpdatePointText();
    }

    private void Update()
    {
        if (skillPointText != null) UpdateSkillPointText();
    }

    void UpdateSkillPointText()
    {
        skillPointText.text = string.Format("Poin : {0}", skillPoint);
    }

    public void UpdateSkillnameText(string text = "")
    {
        skillnameText.text = text;
    }

    public void UpdateSkillInfoText(string text ="")
    {
        skillInfoText.text = text;
    }

    public void UpdatePointText(string text = "")
    {
        pointText.text = text;
    }
    public void Point(int score)
    {
        skillPoint += score;
        foreach (var block in skillBlocks)
        {
            if (HasSkill(block.SkillType)) // ← SkillBlock に SkillType の public getter を作っておく
            {
                block.SetLearnedColor();
            }
            else
            {
                block.CheckActiveBlock();
            }
        }
    }

    public bool HasSkill(SkillType skillType)//listのなかにあるかどうか
    {
        return skillList.Contains(skillType);
    }

    public bool CanLearnSkill(int cost, SkillType skillType)//コストが足りているか
    {
        if (skillPoint < cost) return false;

        if (skillType == SkillType.ATTACK2) return HasSkill(SkillType.ATTACK1);
        if (skillType == SkillType.ATTACK3) return HasSkill(SkillType.ATTACK2);
        if (skillType == SkillType.GROUND) return HasSkill(SkillType.ATTACK1);
        if (skillType == SkillType.KNOCKP1) return HasSkill(SkillType.ATTACK3) && HasSkill(SkillType.GROUND);
        if (skillType == SkillType.KNOCKP2) return HasSkill(SkillType.KNOCKP1);
        if (skillType == SkillType.KNOCKP3) return HasSkill(SkillType.KNOCKP2);

        if (skillType == SkillType.HP2) return HasSkill(SkillType.HP1);
        if (skillType == SkillType.HP3) return HasSkill(SkillType.HP2);
        if (skillType == SkillType.SLASH) return HasSkill(SkillType.HP1);
        if (skillType == SkillType.UNB1) return HasSkill(SkillType.HP3) && HasSkill(SkillType.SLASH);
        if (skillType == SkillType.UNB2) return HasSkill(SkillType.UNB1);
        if (skillType == SkillType.UNB3) return HasSkill(SkillType.UNB2);

        if (skillType == SkillType.GUN2) return HasSkill(SkillType.GUN1);
        if (skillType == SkillType.GUN3) return HasSkill(SkillType.GUN2);
        if (skillType == SkillType.SHOTGUN) return HasSkill(SkillType.GUN1);
        if (skillType == SkillType.SKILL1) return HasSkill(SkillType.GUN3) && HasSkill(SkillType.SHOTGUN);
        if (skillType == SkillType.SKILL2) return HasSkill(SkillType.SKILL1);
        if (skillType == SkillType.SKILL3) return HasSkill(SkillType.SKILL2);

        if (skillType == SkillType.SPEED2) return HasSkill(SkillType.SPEED1);
        if (skillType == SkillType.SPEED3) return HasSkill(SkillType.SPEED2);
        if (skillType == SkillType.RIFLE) return HasSkill(SkillType.SPEED1);
        if (skillType == SkillType.KNOCKS1) return HasSkill(SkillType.SPEED3) && HasSkill(SkillType.RIFLE);
        if (skillType == SkillType.KNOCKS2) return HasSkill(SkillType.KNOCKS1);
        if (skillType == SkillType.KNOCKS3) return HasSkill(SkillType.KNOCKS2);


        return true;
    }

    public void LearnSkill(int cost, SkillType skillType)
    {
        skillList.Add(skillType);
        CheckActiveBlocks();
        skillPoint -= cost;
        UpdateSkillPointText();
    }

    void CheckActiveBlocks()
    {
        foreach (SkillBlock skillBlock in skillBlocks)
        {
            skillBlock.CheckActiveBlock();
        }
    }
}
