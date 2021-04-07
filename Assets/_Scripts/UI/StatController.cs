using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    public Statistics stat;

    public Image[] points;
    public int minPoint = 0;
    public int currentPoint = 0;

    public Color activeColor;
    public Color inActiveColor;
    public Color lockedColor;

    private void OnEnable()
    {
        UpdateUI();
    }

    public void OnMinusClicked()
    {
        if (currentPoint > minPoint)
        {
            currentPoint--;
            currentPoint = Mathf.Max(currentPoint, minPoint);

            StatManager.instance.ReturnPoint();

            UpdateUI();
        }
    }

    public void OnPlusClicked()
    {
        if (StatManager.instance.UsePoint())
        {
            currentPoint++;
            currentPoint = Mathf.Min(currentPoint, 6);

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        for(int i = 0; i < points.Length; i++)
        {
            if (i <= minPoint)
                points[i].color = lockedColor;
            else if (i <= currentPoint)
                points[i].color = activeColor;
            else
                points[i].color = inActiveColor;
        }
    }
}
