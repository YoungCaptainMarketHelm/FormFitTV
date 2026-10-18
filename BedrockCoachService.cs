using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;

namespace FormFitTV;

public class BedrockCoachService
{
    private IAmazonBedrockRuntime? _client;
    private bool _clientFailed;

    private void EnsureClient()
    {
        if (_client != null || _clientFailed) return;

        try
        {
            _client = new AmazonBedrockRuntimeClient(
                "AKIA6ITMK2FPSJAVJID6",      // put new key here only on your machine
                "qx4YpahiUcY4/OKFuEK6aiBMEW9JlDIytYmqJm/x",
                RegionEndpoint.EUNorth1);
        }
        catch
        {
            _clientFailed = true;
            _client = null;
        }
    }

    public async Task<string> GetCoachFeedbackAsync(string exercise, string userMessage)
    {
        var fallbackTips = new[]
        {
            "Keep your chest up and core tight.",
            "Push through your heels and stay controlled.",
            "Don’t let your knees cave inward.",
            "Breathe out on the effort, stay steady.",
            "Full range of motion — no rushing.",
            "Brace your core and keep your back neutral."
        };

        EnsureClient();

        if (_client != null)
        {
            try
            {
                var prompt = $"You are an expert fitness coach on a TV. The user is doing {exercise}. Give one short, clear form tip (max 12 words).";

                var request = new ConverseRequest
                {
                    ModelId = "arn:aws:bedrock:eu-north-1:980553486687:inference-profile/eu.anthropic.claude-haiku-5-5",
                    Messages = new List<Message>
                    {
                        new Message
                        {
                            Role = ConversationRole.User,
                            Content = new List<ContentBlock>
                            {
                                new ContentBlock { Text = prompt }
                            }
                        }
                    },
                    InferenceConfig = new InferenceConfiguration { MaxTokens = 60 }
                };

                var response = await _client.ConverseAsync(request);
                var text = response?.Output?.Message?.Content?.FirstOrDefault()?.Text?.Trim();

                if (!string.IsNullOrWhiteSpace(text))
                    return "AI Coach: " + text;
            }
            catch
            {
                // Model not available or network issue
            }
        }

        var tip = fallbackTips[Random.Shared.Next(fallbackTips.Length)];
        return "AI Coach: " + tip;
    }
}