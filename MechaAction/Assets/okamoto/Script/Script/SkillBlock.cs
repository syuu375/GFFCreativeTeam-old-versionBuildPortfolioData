using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;

public class SkillBlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] SkillType skillType;
    [SerializeField] int cost;
    [SerializeField] string costtext;
    [SerializeField] new string name;
    [SerializeField] string info;
    [SerializeField] GameObject hidePanel;
    public SkillType SkillType => skillType;

    void Start()
    {
        pointText.text = costtext;
        nameText.text = name;
        infoText.text = info;
        CheckActiveBlock();
    }

    public void OnClick()
    {
        //Debug.Log("d");

        // 習得済みなら何もしない
        if (SkillManager.Instance.HasSkill(this.skillType))
        {
            Debug.Log("習得済み");
            return;
        }
            // 習得可能？
        if (SkillManager.Instance.CanLearnSkill(cost, skillType))
        {
            // 習得可能なら習得する：スキルポイントが足りている & 必要スキルを持っている
            SkillManager.Instance.LearnSkill(cost, this.skillType);
            Debug.Log("習得");
            ChangeLearnedBlock(Color.blue);
        }
        else
        {
            // 習得不可能ならログを出す
            Debug.Log("習得不可");
        }
    }

    public void CheckActiveBlock()
    {
        if (SkillManager.Instance.CanLearnSkill(cost, skillType))
        {
            hidePanel.SetActive(false);
        }
        else
        {
            hidePanel.SetActive(true);
        }
    }

    void ChangeLearnedBlock(Color color)
    {
        Image image = GetComponent<Image>();
        image.color = color;
    }

    public void SetLearnedColor()
    {
        GetComponent<Image>().color = Color.blue;
    }

    public void OnCursor()
    {
        SkillManager.Instance.UpdateSkillInfoText(info);
        SkillManager.Instance.UpdateSkillnameText(name);
    }
}
