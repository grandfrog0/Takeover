public class DragInfo
{
    public V2 startPos, lastPos;
    public bool hasObstanceOnThePath;
    public Unit unit;
    public DragInfo(Unit unit) => this.unit = unit;
    public V2 TargetPos => hasObstanceOnThePath ? startPos : lastPos;
}
