using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour {

    public CellSO cellProperties;

    public TMP_Text chosenTypeText;  //ref to Type Text
    public Image chosenCellColorImage; //ref to CColor Image
    public Image chosenGradientColorImage; //ref to GColor Image
    
    public Sprite[] sprites; //Sprites to choose from
    public Color[] colors; //Colors to choose from

    int typeIndex = 0;
    int CColorIndex = 0;
    int GColorIndex = 0;

  
    private void OnEnable()
    {
        UpdateToCurrentSettings();
    }

    private void UpdateToCurrentSettings()
    {
        chosenTypeText.text = cellProperties.m_sprite.name;
        chosenCellColorImage.color = cellProperties.m_aliveColor;
        chosenGradientColorImage.color = cellProperties.m_gradientColor;
    }

    public void UpdateUI()
    {
        chosenTypeText.text = sprites[typeIndex].name;
        chosenCellColorImage.color = colors[CColorIndex];
        chosenGradientColorImage.color = colors[GColorIndex];
    }

    public void NextCellType()
    {
        typeIndex++;
        typeIndex %= sprites.Length;
        UpdateUI();

    }

    public void PrevCellType()
    {
        typeIndex--;
        typeIndex += sprites.Length;
        typeIndex %= sprites.Length;
        UpdateUI();

    }

    public void NextCellColor()
    {
        CColorIndex++;
        CColorIndex %= colors.Length;
        UpdateUI();
    }

    public void PrevCellColor()
    {
        CColorIndex--;
        CColorIndex += colors.Length;
        CColorIndex %= colors.Length;
        UpdateUI();
    }

    public void NextGradientColor()
    {
        GColorIndex++;
        GColorIndex %= colors.Length;
        UpdateUI();
    }

    public void PrevGradientColor()
    {
        GColorIndex--;
        GColorIndex += colors.Length;
        GColorIndex %= colors.Length;
        UpdateUI();
    }

    public void SaveSettings()
    {
        cellProperties.m_aliveColor = colors[CColorIndex];
        cellProperties.m_gradientColor = colors[GColorIndex];
        cellProperties.m_sprite = sprites[typeIndex];
    }
    

}
