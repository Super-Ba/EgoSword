using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int MaxHp;
    public int Hp;

    protected virtual void Attack()
    {

    }

    public virtual void Damage(Vector3 hitSource, int power)
    {

    }
}
