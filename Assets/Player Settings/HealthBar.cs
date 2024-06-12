using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider; // The actual slider that would have the fill componenet and values connected
    public Gradient gradient; // Reference for making the filled bar a gradient rather than one solid color
    public Image fill; // Reference to the image componenet of the bar that would be filled
    
    // Method that will set the max value for the slider
    public void SetMaxHealth(int health) {
        slider.maxValue = health; 
        slider.value = health; 

        fill.color = gradient.Evaluate(1f); 
    }
    
    // A method that will set the health value at the time, so when damage hits, the slider will go down to this value 
    public void SetHealth(int health) {
        slider.value = health; 

        fill.color = gradient.Evaluate(slider.normalizedValue); 
    }
}
