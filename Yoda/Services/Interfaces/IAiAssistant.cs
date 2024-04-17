using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using OpenAiModel.Models;

namespace OpenAiModel.Services.Interfaces
{
    public interface IAiAssistant
    {
        ChatResponseMessage GetCompletion(IList<ChatMessage> chatInboundHistory, ChatMessage userMessage);
    }
}
