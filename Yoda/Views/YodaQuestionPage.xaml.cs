using OpenAiModel.ViewModels;
namespace OpenAiModel.Views;
public partial class YodaQuestionPage : ContentPage
{
    QuestionViewModel _viewModel;

    public YodaQuestionPage(QuestionViewModel vm)
    {
        InitializeComponent();

        _viewModel = vm;

        BindingContext = _viewModel;
    }
}