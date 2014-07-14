using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkypeCore.MessagesFormatter;
using SkypeCore.MessagesFormatter.HTML;

namespace SkypeCore.Digest
{
    public class HtmlDigestProvider
    {
        public string GenerateDigest(IEnumerable<SkypeSearchResult> searchResults, string [] highlightedWords)
        {
            var digest = new StringBuilder();

            digest.Append("<html>");

            if (searchResults.Any())
            {
                IMessagesFormatter messagesFormatter =
                    new HtmlMessagesFormatter(css => digest.AppendFormat("<head><style>{0}</style></head><body>", css));

                searchResults.ToList().ForEach(result =>
                {
                    if (result.Messages.Count > 0)
                    {
                        digest.AppendLine(string.Format("<h3>{0}:</h3>", result.ChatName));
                        digest.Append(messagesFormatter.FormatMessages(result.Messages, highlightedWords));
                        digest.AppendLine("<br />");
                    }
                });
            }
            else
            {
                digest.Append("Sorry, nothing has been found for your query");
            }

            digest.Append("</body></html>");
            return digest.ToString();
        }
    }
}
