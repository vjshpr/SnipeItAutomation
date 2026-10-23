namespace SnipeItTests.Configuration;

public static class TestConfig
{
    public static string BaseUrl => Environment.GetEnvironmentVariable("BASE_URL") ?? "https://demo.snipeitapp.com";
    public static string AdminUsername => Environment.GetEnvironmentVariable("ADMIN_USER") ?? "admin";
    public static string AdminPassword => Environment.GetEnvironmentVariable("ADMIN_PASS") ?? "password";
}
