public class PlacementBoxVerticalHorizontal : PlacementBox
{
    protected override void OnMouseEnter()
    {
        _craftingManager?.SetSpriteVisiableHorizontalVertical(transform);
    }
}
