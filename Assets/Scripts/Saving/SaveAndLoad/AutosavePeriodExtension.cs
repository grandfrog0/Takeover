public static class AutosavePeriodExtension
{
    public static string Name(this AutosavePeriod period) 
        => period switch
        {
            AutosavePeriod.Never => "Never",
            AutosavePeriod.Every30Seconds => "Every 30 seconds",
            AutosavePeriod.EveryMinute => "Every minute",
            AutosavePeriod.Every2Minutes => "Every 2 minutes",
            AutosavePeriod.Every5Minutes => "Every 5 minutes",
            AutosavePeriod.Every15Minutes => "Every 15 minutes",
            AutosavePeriod.Every30Minutes => "Every 30 minutes",
            AutosavePeriod.EveryHour => "Every hour",
            _ => "unsigned value"
        };
    public static int ToSeconds(this AutosavePeriod period)
        => period switch
        {
            AutosavePeriod.Never => 0,
            AutosavePeriod.Every30Seconds => 30,
            AutosavePeriod.EveryMinute => 60,
            AutosavePeriod.Every2Minutes => 120,
            AutosavePeriod.Every5Minutes => 300,
            AutosavePeriod.Every15Minutes => 900,
            AutosavePeriod.Every30Minutes => 1800,
            AutosavePeriod.EveryHour => 3600,
            _ => 0
        };
}
