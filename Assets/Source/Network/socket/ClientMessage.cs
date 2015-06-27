
namespace Caveman.Network
{
    public class ClientMessage
    {
        public ushort Length { get; set; }
        public string Content { get; set; }

        public static ClientMessage MessageWithActionType(ActionType actionType)
        {

        }

    }
}