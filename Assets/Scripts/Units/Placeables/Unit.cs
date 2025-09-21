using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

[RequireComponent(typeof(GridTransform))]
public abstract class Unit : MonoBehaviour, IPlaceable, IInitializable
{
    // Events
    public UnityEvent<float> OnHealthChanged = new();
    public UnityEvent onDead = new();
    public UnityEvent OnDestroyed { get; private set; } = new();

    public string UnitName { get; private set; }

    public bool IsChosen => UnitsManager.IsCurrent(this);
    public GridTransform gridTransform { get; private set; }

    public bool IsDead => Health <= 0;
    private float maxHealth = 1;
    [SerializeField] float _health;
    public float Health 
    { 
        get => _health; 
        private set 
        { 
            _health = value; 
            OnHealthChanged.Invoke(HealthPercent);
        } 
    }
    public float HealthPercent => Health / maxHealth;

    public InitializeOrder Order => InitializeOrder.Unit;
    public virtual void Initialize()
    {
        gridTransform = GetComponent<GridTransform>();
    }
    public virtual void InitUnit(Dictionary<string, string> args)
    {
        Initialize();       // gridTransform
        InitDefaults();     // maxHealth = 1f
        Load(args);         // maxHealth = 100f
        _health = maxHealth;// _health = 100f
    }
    protected virtual void InitDefaults()
    {
        UnitName = "";
        gridTransform.Position = new V2(0, 0);
        maxHealth = 1f;
    }

    public virtual void Place(V2 pos)
    {
        if (!CanPlace(pos, out Unit u))
            throw new ArgumentException();

        gridTransform.MoveToAsync(pos);
    }
    public virtual bool TryPlace(V2 pos)
    {
        if (!CanPlace(pos, out Unit u))
            return false;

        gridTransform.MoveToAsync(pos);

        return true;
    }
    public virtual bool HasObstacle(V2 pos, out Unit u) 
        => GridUnit.HasUnit(pos, out u) && u != this;
    public virtual bool CanPlace(V2 pos, out Unit u)
        => !((GridUnit.HasUnit(pos, out u) && u != this) || gridTransform.HasAnimation || gridTransform.IsMoving);
    public bool HasNeighbor(V2 add, out Unit unit) => GridUnit.HasUnit(gridTransform.Position + add, out unit);
    public bool HasNeighbor(V2 add) => GridUnit.HasUnit(gridTransform.Position + add);
    public bool HasAnyNeighbor()
    {
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            if (EditorKit.DebugIt(HasNeighbor((V2)dir)))
                return true;

        return false;
    }

    public virtual void OnDeselected() { }
    public virtual void OnSelected() { }
    public virtual bool OnClicked()
    {
        //GetDamage(5);
        if (UnitsManager.CurUnit != this)
        {
            UnitsManager.CurUnit = this;
            return true;
        }
        return false;
    }
    public void OnMouseDown()
    {
        if (!Inputs.IsPointerOnUI(includeWorldSpace: true))
            OnClicked();
    }
    protected virtual void OnDestroy() => OnDestroyed.Invoke();

    public virtual void GetDamage(float value)
    {
        if (value <= 0) return;

        Health -= value;

        if (Health <= 0)
        {
            Health = 0;
            OnDead();
        }
    }
    protected virtual void OnDead()
    {
        onDead.Invoke();
        Debug.Log(gameObject.name + " is dead.");
    }

    public virtual string GetValue(string valueName)
    {
        return valueName switch
        {
            "name" => UnitName,
            "position" => gridTransform.Position.ToString(),
            "max_health" => maxHealth.ToString(),
            "health" => Health.ToString(),
            _ => null
        };
    }
    public virtual void SetValue(string valueName, string value)
    {
        switch (valueName)
        {
            case "name":
                UnitName = value;
                break;

            case "position":
                gridTransform.Position = V2.Parse(value);
                break;

            case "max_health":
                maxHealth = float.Parse(value);
                break;

            case "health":
                Health = float.Parse(value);
                break;
        }
    }
    public void Load(Dictionary<string, string> args)
    {
        foreach ((string k, string v) in args)
            SetValue(k, v);
    }
    public virtual Dictionary<string, string> Deload()
    {
        return new()
        {
            ["name"] = UnitName,
            ["position"] = gridTransform.Position.ToString(),
            ["max_health"] = maxHealth.ToString(),
            ["health"] = Health.ToString(),
        };
    }
}
