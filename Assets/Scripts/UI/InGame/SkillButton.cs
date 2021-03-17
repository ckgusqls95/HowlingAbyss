using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Image skillFilter;
    public Text coolTimeCounter; // 남은 쿨타임을 표시할 테스트

    public float coolTime;

    private float currentCoolTime; // 남은 쿨타임을 추적 할 변수

    private bool canUseSkill = true; // 스킬을 사용할 수 있는지 확인하는 변수

    void Start()
    {
        skillFilter.fillAmount = 0; // 처음에 스킬을 가리지 않음
        coolTimeCounter.text = "";
    }

    public void UseSkill(int skillIndex)
    {
        switch(skillIndex)
        {
            case 1:
                GameManager.Instance.player.UseSkillQ();
                break;
            case 2:
                GameManager.Instance.player.UseSkillW();
                break;
            case 3:
                GameManager.Instance.player.UseSkillE();
                break;
            case 4:
                GameManager.Instance.player.UseSkillR();
                break;
        }
        
        if (canUseSkill)
        {
            Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; // 스킬 버튼 가림

            StartCoroutine("CoolTime");
            currentCoolTime = coolTime;
            coolTimeCounter.text = "" + currentCoolTime;

            StartCoroutine("CoolTimeCounter");

            canUseSkill = false;
        }
        else
        {
            Debug.Log("아직 스킬을 사용할 수 없습니다.");
        }
    }

    IEnumerator CoolTime()
    {
        while(skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }

        canUseSkill = true;

        yield break;
    }

    //남은 쿨타임을 계산할 코루틴
    IEnumerator CoolTimeCounter()
    {
        while(currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            currentCoolTime -= 1.0f;
            coolTimeCounter.text = "" + currentCoolTime;
        }
        coolTimeCounter.text = "";
        yield break;
    }
}
