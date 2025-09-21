using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class KeyChains : MonoBehaviour, IInitializable
{
    private static KeyChains inst;
    public static Dictionary<KeyCode, KeyCodeChain> Chains { get; private set; }
    public static void AddDown(KeyCode key, UnityAction a, string description = "")
    {
        if (!Chains.ContainsKey(key))
            Chains[key] = new KeyCodeChain(key);

        Chains[key].AddDownListener(a);

        inst.AddMark(key, a, description);
    }
    public static void RemoveDown(KeyCode key, UnityAction a)
    {
        if (Chains.TryGetValue(key, out KeyCodeChain chain))
        {
            chain.RemoveDownListener(a);

            if (!chain.HasSubsribers)
                inst.RemoveMark(key);
            else if (inst.descriptions[key].Count != 0)
            {
                inst.descriptions[key].Remove(a);
                inst.UpdateMark(key);
            }
        }
    }
    public static void AddUp(KeyCode key, UnityAction a, string description = "")
    {
        if (!Chains.ContainsKey(key))
                Chains[key] = new KeyCodeChain(key);

        Chains[key].AddUpListener(a);

        inst.AddMark(key, a, description);
    }
    public static void RemoveUp(KeyCode key, UnityAction a)
    {
        if (Chains.TryGetValue(key, out KeyCodeChain chain))
        {
            chain.RemoveUpListener(a);

            if (!chain.HasSubsribers)
                inst.RemoveMark(key);
            else if (inst.descriptions[key].Count != 0)
            {
                inst.descriptions[key].Remove(a);
                inst.UpdateMark(key);
            }
        }
    }
    public static void AddHold(KeyCode key, UnityAction a, string description = "")
    {
        if (!Chains.ContainsKey(key))
                Chains[key] = new KeyCodeChain(key);

        Chains[key].AddHoldListener(a);

        inst.AddMark(key, a, description);
    }
    public static void RemoveHold(KeyCode key, UnityAction a)
    {
        if (Chains.TryGetValue(key, out KeyCodeChain chain))
        {
            chain.RemoveHoldListener(a);

            if (!chain.HasSubsribers)
                inst.RemoveMark(key);
            else if (inst.descriptions[key].Count != 0)
            {
                inst.descriptions[key].Remove(a);
                inst.UpdateMark(key);
            }
        }
    }

    [SerializeField] Dict<KeyCode, string> pseudonyms = new();
    [SerializeField] KeyChainMark markPrefab;
    [SerializeField] List<KeyChainMark> marks;
    [SerializeField] Transform marksParent;
    private Dictionary<KeyCode, Dictionary<UnityAction, string>> descriptions;

    public InitializeOrder Order => InitializeOrder.KeyChains;
    public void Initialize()
    {
        inst = this;
        Chains = new();
        descriptions = new();
    }

    private bool TryGetMark(KeyCode key, out KeyChainMark mark)
    {
        foreach (KeyChainMark m in marks)
        {
            if (m.KeyCode == key)
            {
                mark = m;
                return true;
            }
        }

        mark = null;
        return false;
    }
    private void AddMark(KeyCode key, UnityAction action, string description)
    {
        if (pseudonyms.ContainsKey(key))
        {
            if (!descriptions.ContainsKey(key))
                descriptions[key] = new();
            descriptions[key][action] = description;

            if (!TryGetMark(key, out KeyChainMark m))
            {
                KeyChainMark mark = Instantiate(markPrefab, marksParent);

                mark.Init(key, pseudonyms[key], description);
                marks.Add(mark);
                mark.transform.SetAsFirstSibling();
            }
            else UpdateMark(key);
        }
    }
    private void RemoveMark(KeyCode key)
    {
        if (TryGetMark(key, out KeyChainMark m))
        {
            marks.Remove(m);
            descriptions[key].Clear();
            Destroy(m.gameObject);
        }
    }
    private void UpdateMark(KeyCode key)
    {
        if (TryGetMark(key, out KeyChainMark m) && descriptions.TryGetValue(key, out Dictionary<UnityAction, string> descr) 
            && descr != null && Chains.ContainsKey(key) && Chains[key].downSubscribers.Count > 0)
        {
            m.Description = descr[Chains[key].NextDownSubscriber];
            m.transform.SetAsFirstSibling();
        }
    }
}
