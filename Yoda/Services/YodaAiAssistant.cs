using Azure;
using Azure.AI.OpenAI;
using OpenAiModel.Configuration;
using OpenAiModel.Models;
using OpenAiModel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAiModel.Services
{
    public class YodaAiAssistant : IAiAssistant
    {
        private ISettings _settings;
        private const string AssistantBehaviorDescription = "Master Yoda I am. Advice, fun facts and jokes I do tell.";

        public YodaAiAssistant(ISettings settings)
        {
            _settings = settings;
        }

        private IList<ChatRequestMessage> BuildChatContext(IList<ChatMessage> chatInboundHistory, ChatMessage userMessage)
        {
            var chatContext = new List<ChatRequestMessage>();

            chatContext.Add(new ChatRequestSystemMessage(AssistantBehaviorDescription));

            foreach (var chatMessage in chatInboundHistory)
                chatContext.Add(new ChatRequestAssistantMessage(chatMessage.MessageBody));

            chatContext.Add(new ChatRequestUserMessage(userMessage.MessageBody));

            return chatContext;

        }

        public ChatResponseMessage GetCompletion(IList<ChatMessage> chatInboundHistory, ChatMessage userMessage)
        {
            var messages = BuildChatContext(chatInboundHistory, userMessage);

            var client = new OpenAIClient(new Uri(_settings.AzureOpenAiEndPoint), new AzureKeyCredential(_settings.AzureOpenAiKey));
            string deploymentName = "masteryodaopenai";
            string searchIndex = "finder";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                    {
                        new ChatRequestSystemMessage ("You are an AI bot that emulates a Master Yoda writing assistant who speaks in a Yoda style. You offer advice, fun facts and tell jokes. \r\nHere are some example of Master Yoda's style:\r\n - Patience you must have my young Padawan.\r\n - In a dark place we find ourselves, and a little more knowledge lights our way.\r\n - Once you start down the dark path, forever will it dominate your destiny. Consume you, it will."),
                        new ChatRequestUserMessage ("Greetings Young Padawan. Patience you must have, for answers I shall provide."),
                    },

                AzureExtensionsOptions = new AzureChatExtensionsOptions()
                {
                    

                    Extensions =
        {
            new AzureSearchChatExtensionConfiguration()
            {
                SearchEndpoint = new Uri(_settings.AzureSearchEndPoint),
                Authentication = new OnYourDataApiKeyAuthenticationOptions(_settings.AzureSearchKey),

                IndexName = searchIndex,
            },
                    }
                },
                DeploymentName = deploymentName
            };

            foreach (var message in messages)
                chatCompletionsOptions.Messages.Add(message);

            Response<ChatCompletions> response = client.GetChatCompletions(chatCompletionsOptions);

            ChatResponseMessage responseMessage = response.Value.Choices[0].Message;

            return responseMessage;

        }



    }
}
