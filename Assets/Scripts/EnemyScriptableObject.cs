using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy Scriptable Object", order = 0)]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    private new string name;    // Enemy's name
    [SerializeField]
    private float moveSpeed; // Enemy's moving speed
    [SerializeField]
    private Sprite sprite;  // Enemy's look
    [SerializeField]
    private int health;   // Enemy's current health
    [SerializeField]
    private int maxHealth; // Enemy's max health
    [SerializeField]
    private int reward;   // Enemy's reward points

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
        set
        {
            sprite = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public int Reward
    {
        get
        {
            return reward;
        }
        set
        {
            reward = value;
        }
    }
}
