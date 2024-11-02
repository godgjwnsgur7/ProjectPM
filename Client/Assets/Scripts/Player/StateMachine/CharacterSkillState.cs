using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ų�� �Ϲ� �޺� ���ݰ��� ������ �ٸ���
/// � ������ �� �� �� �� ���� ������... ���� ���� ��ų���� �����ϴ� ������ �켱 ����
/// </summary>
/// 
public abstract class CharacterSkillState : CharacterControllerState
{
    protected abstract ENUM_CHARACTER_TYPE skillCharacterType { get; }

    private bool IsValidState => skillCharacterType == characterType;

    public sealed override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStateEnter(animator, animatorStateInfo);
    }

    public sealed override void OnStatePrevUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStatePrevUpdate(animator, animatorStateInfo);
    }

    public sealed override void OnStateLateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStateLateUpdate(animator, animatorStateInfo);
    }

    public sealed override void OnStateExit(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (IsValidState == false)
            return;

        OnSkillStateExit(animator, animatorStateInfo);
    }


    protected override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
    {

    }

    protected abstract void OnSkillStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
    protected abstract void OnSkillStatePrevUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
    protected abstract void OnSkillStateLateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
    protected abstract void OnSkillStateExit(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo);
}
