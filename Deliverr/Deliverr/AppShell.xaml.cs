namespace Deliverr;

public partial class AppShell : Shell
{
    // Om pagina toe te voegen doe je Routing.RegisterRoute(nameof(PaginaNaam), typeof(PaginaNaam)); toevoegen in deze file
    // Dan voeg je builder.Services.AddSingleton<PaginaNaam>(); toe in MauiProgram.cs
    // Voeg <ShellContent Title="PaginaNaam" ContentTemplate="{DataTemplate pages:PaginaNaam}" /> in AppShell.xaml toe
    // Voeg iets van een frame toe met een TapGestureRecognizer (zie MainPage.xaml) om een knop naar de pagina toe te voegen

    public AppShell()
    {
        InitializeComponent();

        // Routing routes voor het navigeren naar andere pagina
        Routing.RegisterRoute(nameof(NaamLogin), typeof(NaamLogin));
        Routing.RegisterRoute(nameof(WachtwoordLogin), typeof(WachtwoordLogin));
        Routing.RegisterRoute(nameof(WelkomPagina), typeof(WelkomPagina));
        Routing.RegisterRoute(nameof(BestellingPagina), typeof(BestellingPagina));
        Routing.RegisterRoute(nameof(AccountPagina), typeof(AccountPagina));

    }
}
