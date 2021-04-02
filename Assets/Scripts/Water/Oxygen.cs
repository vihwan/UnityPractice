using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    [SerializeField] private Text text_currentOxygen;
    [SerializeField] private Image image_gauge;

    [SerializeField] private float totalOxygen;
    private float currentOxygen;

    public float TotalOxygen { get => totalOxygen; set => totalOxygen = value; }
    public float CurrentOxygen { get => currentOxygen; set => currentOxygen = value; }
    public Text Text_currentOxygen { get => text_currentOxygen; set => text_currentOxygen = value; }
    public Image Image_gauge { get => image_gauge; set => image_gauge = value; }
}
