namespace FormFitTV;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnSquatTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ExercisePage("Squat"));
    }

    private async void OnPushupTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ExercisePage("Push-up"));
    }

    private async void OnPlankTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ExercisePage("Plank"));
    }
}