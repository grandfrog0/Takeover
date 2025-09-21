
public enum InitializeOrder
{
    BeforeInitialization,

    DataLoader,
    SettingsManager,

    Inputs,
    KeyChains,

    Camera,
    Buffers,

    UnitsManager,
    InventoryManager,

    UnitUIManager,

    UnitImageManager,
    ItemsSpawner,

    DirectionedSprite,
    DirectionedRenderer,

    Unit,
    UnitGrid,

    ItemsManager,

    MenuManager,

    AfterInitialization
}