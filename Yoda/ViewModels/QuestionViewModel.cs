using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAiModel.Models;
using OpenAiModel.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Platform;

namespace OpenAiModel.ViewModels
{
    public partial class QuestionViewModel : BaseViewModel
    {
        private IAiAssistant _assistant;

        private ObservableCollection<ChatMessage> _chatHistory;

        public ObservableCollection<ChatMessage> ChatHistory
        {
            get { return _chatHistory; }
            set
            {
                _chatHistory = value;

                OnPropertyChanged();
            }
        }

        private string _currentQuestion;
        public string CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                _currentQuestion = value;

                OnPropertyChanged();
            }
        }


        //      public ICommand AskQuestionCommand { get; set; }

        public QuestionViewModel(IAiAssistant assistant)
        {
            _assistant = assistant;

            ChatHistory = new ObservableCollection<ChatMessage>();
            ChatHistory.Add(new ChatMessage { MessageType = Enums.ChatMessageTypeEnum.Inbound, MessageBody = "Hello, how can I help you today?" });
        }

        [RelayCommand]
        public async Task ChatSelected(ChatMessage message)
        {
            var navigationParameter = new Dictionary<string, object>
     {
         { "Response", message }
     };
            await Shell.Current.GoToAsync($"Answer", navigationParameter);

        }

        [RelayCommand]
        public async Task AskQuestion(ITextInput view, CancellationToken token)
        {
            /*  QuestionResponseModel model = new QuestionResponseModel();
              model.Answer = "Hello, World!";
              var navigationParameter = new Dictionary<string, object>
      {
          { "Response", model }
      };
              await Shell.Current.GoToAsync($"loadsheddinganswer", navigationParameter);
            */



            var inboundMessages = ChatHistory.Where(x => x.MessageType == Enums.ChatMessageTypeEnum.Inbound).ToList();

            var currentChatMessage = new ChatMessage { MessageType = Enums.ChatMessageTypeEnum.Outbound, MessageBody = CurrentQuestion };

            try
            {
                var response = _assistant.GetCompletion(inboundMessages, currentChatMessage);
                ChatHistory.Add(currentChatMessage);

                var responseChatMessage = new ChatMessage { MessageType = Enums.ChatMessageTypeEnum.Inbound, MessageBody = response.Content };
                ChatHistory.Add(responseChatMessage);


                CurrentQuestion = string.Empty;
            }
            catch (Exception ex)
            {

            }
            bool isSuccessful = await view.HideKeyboardAsync(token);
        }

    }
}
