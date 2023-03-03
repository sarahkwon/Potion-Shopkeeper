using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationSystem : MonoBehaviour
{
    public float reputation = 50f; //reputation goes from 0 - 100
    //lowest customer rate is one every 15 seconds, highest is one every 5
    //each point of reputation is -0.1f to customer rate
    public Slider slider;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlider(float value) {
        slider.value = value;
    }


}
