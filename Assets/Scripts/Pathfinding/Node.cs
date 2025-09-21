using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public V2 Pos { get; set; }
    public int GCost { get; set; }  // from start
    public int HCost { get; set; }  // to end
    public int FCost => GCost + HCost;
    public Node Parent { get; set; }

    public override string ToString() => $"Pos: {Pos}, Cost: {GCost} + {HCost} = {FCost}, Parent: {Parent?.Pos}";
}
