using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System.Configuration;

Console.WriteLine("Starting OAuth login with Google...\n");

AuthenticateUser().Wait();

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

/// <summary>
/// Authenticates the user using Google OAuth 2.0
/// and fetches profile information using access token
/// </summary>
static async Task AuthenticateUser()
{
    string? clientId = ConfigurationManager.AppSettings["ClientId"];
    string? clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

    ClientSecrets secrets = new ClientSecrets
    {
        ClientId = clientId,
        ClientSecret = clientSecret
    };

    string[] scopes =
    {
        "openid",
        "email",
        "profile"
    };

    UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
        secrets,
        scopes,
        "user",
        CancellationToken.None,
        new FileDataStore("OAuthDemoTokens", true)
    );

    Console.WriteLine("User authenticated successfully!\n");

    await GetUserProfile(credential.Token.AccessToken);
}

/// <summary>
/// Calls Google UserInfo endpoint using access token
/// </summary>
static async Task GetUserProfile(string accessToken)
{
    using (HttpClient client = new HttpClient())
    {
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response =
            await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");

        string json = await response.Content.ReadAsStringAsync();
        JObject data = JObject.Parse(json);

        Console.WriteLine("User Profile Details:");
        Console.WriteLine("---------------------");
        Console.WriteLine("Name   : " + data["name"]);
        Console.WriteLine("Email  : " + data["email"]);
        Console.WriteLine("Picture: " + data["picture"]);
    }
}

