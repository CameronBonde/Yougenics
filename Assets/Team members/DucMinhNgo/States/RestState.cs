using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Minh
{
public class RestState : Basestate
{
    public float regenEnergy = 5f;

    private void OnEnable()
    {

        GetComponent<Renderer>().material.color = Color.cyan;
    }

    private void OnDisable()
    {
        //Deactive
        GetComponent<Renderer>().material.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Energy>().energyAmount += regenEnergy * Time.deltaTime;
    }
}
}

