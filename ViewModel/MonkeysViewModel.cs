using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    MonkeyService monkeyService;

    IConnectivity connectivity;
    IGeolocation geolocation;   
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }
    [RelayCommand]
    async Task GetClosestMonkeysAsync()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if(location is null)
            {
                location = await geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30),

                    }
                    );
            }

            if (location is null)
                return;

            var first = Monkeys.OrderBy(m =>
            location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Miles)
            ).FirstOrDefault();

            if (first is null)
                return;

            await Shell.Current.DisplayAlertAsync("Closest Monkey", $"The closest monkey is {first.Name} in {first.Location}", "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlertAsync("Error!",
                $"Unable to get closest monkey: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    async Task GoToDetails(Monkey monkey)
    {
        if (monkey == null)
            return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Monkey", monkey }
        });
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if(connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlertAsync("No connectivity!", "Please check your internet connection and try again.", "OK");
                return;
            }
            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if (Monkeys.Count != 0)
                Monkeys.Clear();

            foreach (var monkey in monkeys)
                Monkeys.Add(monkey);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlertAsync("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }
}