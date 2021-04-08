using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class AlignmentSystem : MonoBehaviour
{
    public UnityEvent<int> onAligneChanged;
    [SerializeField] private float Morals = 0F;
    [SerializeField] private float Leanings = 0F;
    [SerializeField] private float Sexiness = 0F;

    void Start(){
        onAligneChanged.AddListener(UpdateMorals);
        onAligneChanged.AddListener(UpdateLeaning);
        onAligneChanged.AddListener(UpdateSexiness);
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            TestAddPoints();
        }
    }
    public void UpdateMorals(int points){
        Debug.Log("Update Morals " + points);
        //Morals = Morals + points;
    }
        public void UpdateLeaning(int points){
        Debug.Log("Update Leanings " + points);
        //Leanings = Leanings + points;
    }
        public void UpdateSexiness(int points){
        Debug.Log("Update Sexiness " + points);
        //Sexiness = Sexiness + points;
    }
    public void TestAddPoints(){
        onAligneChanged.Invoke(5);
    }
}