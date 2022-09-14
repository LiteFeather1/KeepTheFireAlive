using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryAbstractUi : MonoBehaviour
{
    [SerializeField] protected Image _myImage;
    [SerializeField] protected TMP_Text _text;

    public virtual void SetMe(float amount)
    {
        if(amount == 0)
        {
            _myImage.color = Color.gray;
            _text.text = "";
        }
        else
        {
            _myImage.color = Color.white;
        }
    }
}