using System.ComponentModel;

public class GloData
{
    public static readonly int ColumnCount = 5;
    public static readonly float ColumnOffset = 1.4f;
    public static readonly float RowOffset = 1.3f;
    public static readonly string OstrichSpriteName = "鴕鳥靜止";
}

public enum BirdType
{
    [Description("老鷹")]
    Eagle = 1,
    [Description("鴕鳥")]
    Ostrich,
    [Description("麻雀")]
    Sparrow,
    [Description("文鳥")]
    WenBird
}

public enum ActionType
{
    [Description("左傳紙條")]
    SendLeft,
    [Description("右傳紙條")]
    SendRight,
    [Description("前傳紙條")]
    SendForward,
    [Description("後傳紙條")]
    SendBack,
    [Description("伸左手")]
    RaiseLeftHand, 
    [Description("伸右手")]
    RaiseRightHand,
    [Description("往前伸")]
    RaiseForwardHand,
    [Description("往後伸")]
    RaiseBackHand,
    [Description("左聊天")]
    TalkingToLeft,
    [Description("右聊天")]
    TalkingToRight,
    [Description("呆滯")]
    Idle
}
