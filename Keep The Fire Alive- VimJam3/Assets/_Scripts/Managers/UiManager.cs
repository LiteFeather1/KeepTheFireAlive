using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject _craftingScreen;

    [Header("PopUp Window")]
    [SerializeField] private RectTransform _popUpWindow;
    [SerializeField] private Image[] _materialImage;
    [SerializeField] private Sprite[] _materialSprite;
    [SerializeField] private TMP_Text[] _materialText;
    [SerializeField] private TMP_Text _description;

    //[Header("InventoryUi")]
    //[SerializeField] private something;

    private static UiManager Instance => GameManager.Instance.Ui;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            if (_craftingScreen.activeInHierarchy)
            {
                SwitchCraftingMenuActive();
                DeactivatePopUpWindow();
                Time.timeScale = 1;
            }
    }

    public void SwitchCraftingMenuActive()
    {
        bool active = _craftingScreen.activeInHierarchy;
        _craftingScreen.SetActive(!active);
        if (!active)
            Time.timeScale = 0;
    }

    public void ActivatePopUpWindow(RectTransform target, Materials[] materialsToShow, int[] amountNeeded, string description)
    {
        _popUpWindow.gameObject.SetActive(true);
        _popUpWindow.position = target.position;
        for (int i = 0; i < materialsToShow.Length; i++)
        {
            _materialImage[i].sprite = _materialSprite[(int)materialsToShow[i]];
            _materialImage[i].gameObject.SetActive(true);

            int inventoryAmount = InventorySystem.Instance.GetItemAmount(materialsToShow[i]);
            inventoryAmount = Mathf.Clamp(inventoryAmount, 0, amountNeeded[i]);
            _materialText[i].text = $"{inventoryAmount}  /  {amountNeeded[i]}";
            _materialText[i].color = inventoryAmount >= amountNeeded[i] ? Color.green : Color.red;

            _description.text = description;
        }
    }

    public void DeactivatePopUpWindow()
    {
        _popUpWindow.gameObject.SetActive(false);
        foreach (var item in _materialImage)
        {
            item.gameObject.SetActive(false);
        }
    }
}
