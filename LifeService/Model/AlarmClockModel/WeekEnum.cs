using System.ComponentModel;

namespace LifeService.Model.AlarmClockModel
{
    /// <summary>
    /// 星期枚举
    /// </summary>
    internal enum WeekEnum
    {
        [Description("星期日")] 星期日 = 0,
        [Description("星期一")] 星期一 = 1,
        [Description("星期二")] 星期二 = 2,
        [Description("星期三")] 星期三 = 3,
        [Description("星期四")] 星期四 = 4,
        [Description("星期五")] 星期五 = 5,
        [Description("星期六")] 星期六 = 6
    }
}
