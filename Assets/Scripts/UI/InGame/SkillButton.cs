using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Image skillFilter;
    public Text coolTimeCounter; // 남은 쿨타임을 표시할 테스트
 
    void Start()
    {
        skillFilter.fillAmount = 0; // 처음에 스킬을 가리지 않음
        coolTimeCounter.text = "";
    }

    public void UseSkill(int skillIndex)
    {
        //if (currentCoolTime <= 0.0f)
        //{
        //    switch (skillIndex)
        //    {
        //        case 1:
        //            GameManager.Instance.player.UseSkillQ();
        //            break;
        //        case 2:
        //            GameManager.Instance.player.UseSkillW();
        //            break;
        //        case 3:
        //            GameManager.Instance.player.UseSkillE();
        //            break;
        //        case 4:
        //            GameManager.Instance.player.UseSkillR();
        //            break;
        //    }
        //}
    }

}
