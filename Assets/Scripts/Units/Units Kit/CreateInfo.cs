using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public struct CreateInfo
{
    public string unitName;
    public int count;
    public int energyCost;
    public int time;
    public int cost;

    public override string ToString()
        => $"unitName: {unitName}; count: {count}; time: {time}; energyCost: {energyCost}; cost: {cost}";

    public static CreateInfo Parse(string str)
    {
        CreateInfo info = new CreateInfo();

        if (!string.IsNullOrWhiteSpace(str))
        {
            string[] pairs = str.Split(";");

            foreach (string pair in pairs)
            {
                string[] kv = pair.Trim().Split(":");
                if (kv.Length == 2)
                {
                    string key = kv[0].Trim();
                    string value = kv[1].Trim();

                    switch (key.Trim())
                    {
                        case "unitName":
                            info.unitName = value.Trim();
                            break;

                        case "count":
                            if (int.TryParse(value, out int c))
                                info.count = c;
                            else Debug.Log("wrong count format!");
                                break;

                        case "time":
                            if (int.TryParse(value, out int t))
                                info.time = t;
                            else Debug.Log("wrong time format!");
                            break;

                        case "energyCost":
                            if (int.TryParse(value, out int e))
                                info.energyCost = e;
                            else Debug.Log("wrong energyCost format!");
                            break;

                        case "cost":
                            if (int.TryParse(value, out int m))
                                info.cost = m;
                            else Debug.Log("wrong cost format!");
                            break;

                        default:
                            Debug.Log("unsigned argument: " + key);
                            break;
                    }
                }
            }
        }

        return info;
    }
}
