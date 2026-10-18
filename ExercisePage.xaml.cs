namespace FormFitTV;

public partial class ExercisePage : ContentPage
{
    private readonly string _exerciseName;
    private readonly BedrockCoachService _coach = new();
    private int _reps = 0;
    private int _seconds = 0;
    private bool _timerRunning;

    public ExercisePage(string exerciseName)
    {
        InitializeComponent();
        _exerciseName = exerciseName;
        ExerciseTitle.Text = exerciseName;
        StartTimer();
    }

    private void StartTimer()
    {
        _timerRunning = true;
        Application.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (!_timerRunning) return false;
            _seconds++;
            var ts = TimeSpan.FromSeconds(_seconds);
            MainThread.BeginInvokeOnMainThread(() =>
                TimerLabel.Text = $"{ts.Minutes:00}:{ts.Seconds:00}");
            return true;
        });
    }

    private async void OnAskCoachClicked(object sender, EventArgs e)
    {
        try
        {
            CoachFeedback.Text = "AI Coach: Thinking...";
            var feedback = await _coach.GetCoachFeedbackAsync(_exerciseName, "How is my form?");
            CoachFeedback.Text = feedback;
            await SpeakAsync(feedback);
        }
        catch
        {
            var msg = "AI Coach: Keep your core tight and move with control.";
            CoachFeedback.Text = msg;
            await SpeakAsync(msg);
        }
    }

    private async void OnRepDoneClicked(object sender, EventArgs e)
    {
        _reps++;
        RepsLabel.Text = $"Reps: {_reps}";
        await SpeakAsync($"Rep {_reps} complete.");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        _timerRunning = false;
        await Navigation.PopAsync();
    }

    private async Task SpeakAsync(string text)
    {
        try { await TextToSpeech.Default.SpeakAsync(text); }
        catch { }
    }
}