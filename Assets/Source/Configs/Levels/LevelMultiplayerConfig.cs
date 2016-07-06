using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Configs.Levels
{
  [DataContract]
  public  class LevelMultiplayerConfig : ISettings
  {
    public LevelMultiplayerConfig(string name, int roundTime)
    {
      this.roundTime = roundTime;
      this.name = name;
    }

    [DataMember] private string name;
    [DataMember] private int roundTime;

    public string Name
    {
      get { return name; }
    }

    public int RoundTime
    {
      get { return roundTime; }
    }
  }
}