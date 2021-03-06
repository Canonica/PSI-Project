﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraController : MonoBehaviour {
    public GameObject player;       
    private Vector3 offset;
    private Vector3 initialPos;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
        initialPos = transform.position;
    }

    void LateUpdate()
    {
        if(ContextManager.instance.CompareContext(ContextManager.GameContext.Diving) || ContextManager.instance.CompareContext(ContextManager.GameContext.Ascent))
        {
            transform.position = new Vector3(0, player.transform.position.y + offset.y, -10 + offset.z);
        }
    }

    public void TransitionAToS(float duration)
    {
        //transform.DOMove(new Vector3(transform.position.x, transform.position.y + 6, transform.position.z), duration).SetEase(Ease.InOutSine).OnComplete(() => offset = transform.position - player.transform.position);
    }

    public void TransitionDToA(float duration)
    {
        transform.DOMove(new Vector3(0, transform.position.y + 6 , transform.position.z), duration).SetEase(Ease.InOutSine).OnComplete(()=> offset = transform.position - player.transform.position);
    }



}
