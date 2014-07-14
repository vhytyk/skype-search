using System.Collections.Generic;

namespace SkypeCore.MessagesFormatter
{
    public interface IMessagesFormatter
    {
        string FormatMessages(List<SkypeMessage> messages, string[] highlightedWords);
    }
}
