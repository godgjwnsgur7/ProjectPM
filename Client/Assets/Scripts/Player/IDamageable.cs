using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUM_DAMAGE_TYPE
{
    None = 0,
    Stand, // ���ĵ� ���� Ÿ��
    Airborne, // ���� Ÿ��, ������ ��
    JustDamage, // ���۾Ƹ� ������ �������� ���� Ÿ��
    GuardDamage, // ���� �� Ÿ��
}

[Serializable]
public struct DamageInfo
{
    [Header("[������ Ÿ��]")]
    [SerializeField] public ENUM_DAMAGE_TYPE type;

    [Header("[�⺻ ������ ��]")]
    [SerializeField] public int amount;

    [Header("[�о�� ��]")]
    [SerializeField] public float knockBackPower;

    [Header("[���� �ð� (������ Ÿ���� Stand�� ��쿡�� ��ȿ)]")]
    [SerializeField] public float stiffTime;

    [Header("[���� ���� (������ Ÿ���� Airborne�� ��쿡�� ��ȿ)]")]
    [SerializeField] public float upperHeight;
}

public interface IDamageable
{
	bool OnHit(PlayerCharacterController attacker, DamageInfo damageInfo);
}