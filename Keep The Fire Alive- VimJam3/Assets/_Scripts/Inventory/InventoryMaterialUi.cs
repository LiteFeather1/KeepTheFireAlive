public class InventoryMaterialUi : InventoryAbstractUi
{
    public override void SetMe(float amount)
    {
        _text.text = $"{amount}";
        base.SetMe(amount);
    }
}
