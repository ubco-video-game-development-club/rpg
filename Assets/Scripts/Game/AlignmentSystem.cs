using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class AlignmentSystem : MonoBehaviour
{    
    public UnityEvent<int> onAligneChanged;
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
    }
        public void UpdateLeaning(int points){
        Debug.Log("Update Leanings " + points);
    }
        public void UpdateSexiness(int points){
        Debug.Log("Update Sexiness " + points);
    }
    public void TestAddPoints(){
        onAligneChanged.Invoke(5);
    }
}