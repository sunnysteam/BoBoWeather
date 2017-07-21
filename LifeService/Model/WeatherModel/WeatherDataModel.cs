namespace LifeService.Model.Weathermodel
{
    internal class WeatherDataModel
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public Results[] Results { get; set; }

        /// <summary>
        /// 错误状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string Status_Code { get; set; }
    }

    /// <summary>
    /// 返回结果
    /// </summary>
    internal class Results
    {
        /// <summary>
        /// 地址信息
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// 天气预报
        /// </summary>
        public Daily[] Daily { get; set; }

        /// <summary>
        /// 实时气温
        /// </summary>
        public Now Now { get; set; }

        /// <summary>
        /// 数据更新时间（该城市的本地时间）
        /// </summary>
        public string Last_Update { get; set; }
    }

    /// <summary>
    /// 城市信息
    /// </summary>
    internal class Location
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string Path { get; set; }

        public string TimeZone { get; set; }

        public string TimeZone_Offset { get; set; }
    }

    /// <summary>
    /// 天气预报
    /// </summary>
    internal class Daily
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 白天天气现象文字
        /// </summary>
        public string Text_Day { get; set; }

        /// <summary>
        /// 白天天气现象代码
        /// </summary>
        public string Code_Day { get; set; }

        /// <summary>
        /// 晚间天气现象文字
        /// </summary>
        public string Text_Night { get; set; }

        /// <summary>
        /// 晚间天气现象代码
        /// </summary>
        public string Code_Night { get; set; }

        /// <summary>
        /// 当天最高温度
        /// </summary>
        public string High { get; set; }

        /// <summary>
        /// 当天最低温度
        /// </summary>
        public string Low { get; set; }

        /// <summary>
        /// 降水概率，范围0~100，单位百分比
        /// </summary>
        public string Precip { get; set; }

        /// <summary>
        /// 风向文字
        /// </summary>
        public string Wind_Direction { get; set; }

        /// <summary>
        /// 风向角度，范围0~360
        /// </summary>
        public string Wind_Direction_Degree { get; set; }

        /// <summary>
        /// 风速，单位km/h（当unit=c时）、mph（当unit=f时）
        /// </summary>
        public string Wind_Speed { get; set; }

        /// <summary>
        /// 风力等级
        /// </summary>
        public string Wind_Scale { get; set; }
    }

    /// <summary>
    /// 实时天气
    /// </summary>
    internal class Now
    {
        /// <summary>
        /// 天气现象文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 天气现象代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 温度，单位为c摄氏度或f华氏度
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        /// 体感温度，单位为c摄氏度或f华氏度
        /// </summary>
        public string Feels_like { get; set; }

        /// <summary>
        /// 气压，单位为mb百帕或in英寸
        /// </summary>
        public string Pressure { get; set; }

        /// <summary>
        /// 相对湿度，0~100，单位为百分比
        /// </summary>
        public string Humidity { get; set; }

        /// <summary>
        /// 能见度，单位为km公里或mi英里
        /// </summary>
        public string Visibility { get; set; }

        /// <summary>
        /// 风向文字
        /// </summary>
        public string Wind_Direction { get; set; }

        /// <summary>
        /// 风向角度，范围0~360，0为正北，90为正东，180为正南，270为正西
        /// </summary>
        public string Wind_Direction_Degree { get; set; }

        /// <summary>
        /// 风速，单位为km/h公里每小时或mph英里每小时
        /// </summary>
        public string Wind_Speed { get; set; }

        /// <summary>
        /// 风力等级，请参考：http://baike.baidu.com/view/465076.htm
        /// </summary>
        public string Wind_Scale { get; set; }

        /// <summary>
        /// 云量，范围0~100，天空被云覆盖的百分比 #目前不支持中国城市#
        /// </summary>
        public string Clouds { get; set; }

        /// <summary>
        /// 露点温度，请参考：http://baike.baidu.com/view/118348.htm #目前不支持中国城市#
        /// </summary>
        public string Dew_Point { get; set; }
    }
}
