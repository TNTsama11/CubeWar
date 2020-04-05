using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : UIBase
{
    public Button skillFullPower;

    public Text hgText;

    private Text fullPowerText;

    private bool isSkill = false;

    void Awake()
    {
        Bind();
        skillFullPower.onClick.AddListener(OnFullPowerClick);
    }

    private void Start()
    {
        fullPowerText = skillFullPower.transform.Find("Text").GetComponent<Text>();
        fullPowerText.gameObject.SetActive(false);
    }

    private void OnFullPowerClick()
    {
        if (isSkill)
        {
            return;
        }
        isSkill = true;
        skillFullPower.interactable = false;
        Dispatch(AreaCode.SKILL, SkillEvents.SKILL_DO_SKILL, SkillType.FullPower);
        fullPowerText.gameObject.SetActive(true);
        StartCoroutine(ResetFullPower());
    }

    private void Update()
    {
        if (int.Parse(hgText.text) < 50)
        {
            skillFullPower.interactable = false;
        }
        else
        {
            if (isSkill)
            {
                return;
            }
            skillFullPower.interactable = true;
        }
    }

    IEnumerator ResetFullPower()
    {
        int t = 10;
        while (true)
        {
            if (t <= 0)
            {
                skillFullPower.interactable = true;
                fullPowerText.gameObject.SetActive(false);
                skillFullPower.transform.Find("Mask").GetComponent<Image>().fillAmount = 0;
                isSkill = false;
                break;
            }
            fullPowerText.text = t.ToString();
            skillFullPower.transform.Find("Mask").GetComponent<Image>().fillAmount = t / 10f;
            t--;
            yield return new WaitForSeconds(1f);
        }
    }
}
