namespace MonkeyFinder.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    private bool isBusy;
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(ref isBusy, value);
    }

    private string title;
    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public bool IsNotBusy => !IsBusy;
}