public class PlacementBoxVerticalHorizontal : PlacementBox
{
    protected override void OnMouseEnter()
    {
        print(transform.eulerAngles.z);
        _craftingManager?.SetSpriteVisiableHorizontalVertical(transform);
    }
}
