using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxe : MonoBehaviour
{
    [SerializeField] private int _durability;
    private bool _isActive;
    public bool IsActive => _isActive;

    public void SetNewAxe()
    {
        _isActive = true;
        _durability = 16;
    }

    public void SwingAxe()
    {
        if (_durability > 0)
        {
            _durability--;
            if (_durability == 0)
            {
                BreakAxe();
            }
            print("AxeSwong");
        }
    }

    private void BreakAxe()
    {
        print("AxeBroke");
        _isActive = false;
    }
}
