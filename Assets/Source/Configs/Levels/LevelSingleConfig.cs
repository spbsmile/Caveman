using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Configs.Levels
{
  [DataContract]
  public class LevelSingleConfig : ISettings
  {
    public LevelSingleConfig(string name, int roundTime, int botsCount, string[] botsName)
    {
      this.name = name;
      this.roundTime = roundTime;
      this.botsCount = botsCount;
      this.botsName = botsName;
    }

    [DataMember]
    private string name;
    [DataMember]
    private int roundTime;
    [DataMember]
    private int botsCount;
    [DataMember]
    private string[] botsName;

    public string Name { get { return name; } }

    public int RoundTime { get { return roundTime; } }

    public int BotsCount { get { return botsCount; }}

    public string[] BotsName { get { return botsName; }}

  }
}