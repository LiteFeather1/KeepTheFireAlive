using System;
public class InventoryItemUi : InventoryAbstractUi
{
    public void SetMe(float amount, float maxAmount)
    {
        _text.text = (amount / maxAmount * 100).ToString("00") + "%";
        base.SetMe(amount);
    }
}