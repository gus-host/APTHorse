using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarFilling 
{
    public float DrainMaxValue=50;
    public float FillMaxValue=50;
 public  IEnumerator  UI_barDrain(float MaxValue,Image BarImage,float currentval,float totalValue,float Diff)          //reset drainmaxvalue before calling this method
    {
     
      
       float Dec_stamina = 0;
       
        while (Dec_stamina < Diff)
       {
           Debug.Log("CurrentStamina.....");
           Dec_stamina += .05f;
           if (BarImage != null)
           {
               BarImage.fillAmount = (MaxValue - Dec_stamina) / totalValue;
           }

           yield return new WaitForSeconds(.01f);

       }
        MaxValue -= Diff;
        DrainMaxValue = MaxValue;

    }
 

public IEnumerator UI_BarFill(float maxValue,Image Barfillimage,float TotalValue)
    {
        float Dec_stamina = 0;

        while (Dec_stamina < 5)
        {
            Debug.Log("CurrentStamina.....");
            Dec_stamina += .05f;
            if (Barfillimage != null)
            {
                Barfillimage.fillAmount = (maxValue + Dec_stamina) / TotalValue;


            }

            yield return new WaitForSeconds(.01f);

        }
        maxValue += 5;
        FillMaxValue = maxValue;

    }

}

