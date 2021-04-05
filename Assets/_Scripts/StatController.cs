using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    public Image[] points;
    public int minPoint = 0;
    public int currentPoint = 0;

    public Color activeColor;
    public Color inActiveColor;

    public void OnMinusClicked()
    {
        currentPoint--;
        currentPoint = Mathf.Max(currentPoint, minPoint);


    }

    public void OnPlusClicked()
    {
        currentPoint++;
        currentPoint = Mathf.Min(currentPoint, 6);


    }

    private void UpdateUI()
    {
        for(int i = 0; i < points.Length; i++)
        {
            if (i <= currentPoint)
                points[i].color = activeColor;
            else
                points[i].color = inActiveColor;
        }
    }
}
