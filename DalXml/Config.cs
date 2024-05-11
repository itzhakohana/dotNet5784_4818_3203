namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static bool IsRealTimeClock 
    { 
        get => XMLTools.GetBool(s_data_config_xml, "isRealTimeClock"); 
        set => XMLTools.SetBool(s_data_config_xml, "isRealTimeClock", value);
    }
    internal static DateTime? StartDate 
    { 
        get => XMLTools.GetDate(s_data_config_xml, "startDate"); 
        set => XMLTools.SetDate(s_data_config_xml, "startDate", value); }
    internal static DateTime? EndDate 
    {
        get => XMLTools.GetDate(s_data_config_xml, "endDate");
        set => XMLTools.SetDate(s_data_config_xml, "endDate", value);
    }
    internal static DateTime? CurrentDate 
    {
        get => XMLTools.GetDate(s_data_config_xml, "currentDate");
        set => XMLTools.SetDate(s_data_config_xml, "currentDate", value);
    }

}
