﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour {

    public int maxLife;
    private int currentLife;
	// Use this for initialization

	void Start ()
    {
        currentLife = maxLife;
	}
	
    public void SubstractLife(Fish fish, int amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Max(0, currentLife);
        if(currentLife <= 0)
        {
            ContextManager.instance.SwitchContext(ContextManager.GameContext.TransitionDToA);
            fish.GetCaught(this.transform);
        }
    }

    public void AddLife(int amount)
    {
        currentLife += amount;
        currentLife = Mathf.Min(currentLife, maxLife);
    }

}
