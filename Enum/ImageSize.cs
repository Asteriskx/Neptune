namespace Neptune.Enum
{
    /// <summary>
    /// image のサイズの定義を行います。
    /// </summary>
    public enum ImageSize
    {
        Minimum = 0,       
        Normal,    
        Bigger    
    };

    /// <summary>
    /// 認証パラメータ設定時の 配列指定に使用します。
    /// </summary>
    public enum SwitchType
    {
        CONSUMER_KEY = 0,
        CONSUMER_KEY_S,
        ACCESS_TOKEN,
        ACCESS_TOKEN_S
    }
}
