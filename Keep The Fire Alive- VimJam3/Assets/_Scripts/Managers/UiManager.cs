using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject _craftingScreen;
    [SerializeField] private GameObject _pauseScreen;

    [Header("PopUp Window")]
    [SerializeField] private RectTransform _popUpWindow;
    [SerializeField] private Image[] _materialImage;
    [SerializeField] private Sprite[] _materialSprite;
    [SerializeField] private TMP_Text[] _materialText;
    [SerializeField] private TMP_Text _description;

    [Header("Texts")]
    [SerializeField] private TMP_Text _warningText;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private TMP_Text _textInventoryFeed;

    [Header("Player Stats")]
    [SerializeField] private Image _fireFire;
    [SerializeField] private Image _fireWetness;

    public static UiManager Instance => GameManager.Instance.Ui;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (_craftingScreen.activeInHierarchy)
            {
                SwitchCraftingMenuActive();
                DeactivatePopUpWindow();
                Time.timeScale = 1;
            }
            else
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 0;
                    _pauseScreen.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    _pauseScreen.SetActive(false);;
                }
            }
    }

    public void TimeToDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        _time.text = "Time : " + $"{System.Environment.NewLine}{string.Format("{0:00} : {1:00}", minutes, seconds)}";
    }

    public void SwitchCraftingMenuActive()
    {
        bool active = _craftingScreen.activeInHierarchy;
        _craftingScreen.SetActive(!active);
        _pauseScreen.SetActive(!active);
        if (!active)
        {
            Time.timeScale = 0;
            CraftingManager.Instance.Check();
        }
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

    public void WarningText(string textToShow, float timeToShow)
    {
        _warningText.text = textToShow;
        _warningText.color = Color.white;
        StopCoroutine(nameof(ShowWarningText));
        StartCoroutine(ShowWarningText(timeToShow));
    }

    public void WarningText(string textToShow, float timeToShow, Color color)
    {
        WarningText(textToShow, timeToShow);
        _warningText.color = color;
    }

    private IEnumerator ShowWarningText(float timeToShow)
    {
        _warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeToShow);
        _warningText.gameObject.SetActive(false);
    }

    public void FireFireDisplay(float currentFire)
    {
        _fireFire.fillAmount = currentFire / 100;
    }

    public void FireWetnessDisplay(float currentWetness)
    {
        _fireWetness.fillAmount = currentWetness / 100;
    }

    public void DisplayInventoryFeed(string text)
    {
        _textInventoryFeed.text = text;
    }

    public void HideInventoryFeed()
    {
        _textInventoryFeed.text = "";
    }
}
