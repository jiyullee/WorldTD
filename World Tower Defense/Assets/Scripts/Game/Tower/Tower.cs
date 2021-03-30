using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Tower : MonoBehaviour
{
    protected float range;
    protected float speed;
    protected float attack; 
    protected float defense;

    protected int cost;
    protected int grade;
    protected string synergy;
    abstract public void Attack();
}
