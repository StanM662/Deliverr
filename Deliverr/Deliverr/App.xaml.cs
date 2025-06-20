using Deliverr;

namespace Deliverr;

public partial class App : Application
{
    [Obsolete]
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        Device.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.GoToAsync(nameof(NaamLogin));
        });
    }

}
