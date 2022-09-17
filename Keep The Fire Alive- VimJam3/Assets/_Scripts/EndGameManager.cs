using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _endText;
    [SerializeField] private TMP_Text _timeLeftText;

    private void Start()
    {
        _endText.text = Utils.EndGameLostText;
        _timeLeftText.text = Utils.TimeLeft;
    }

    public void ButtonReplay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
