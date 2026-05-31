namespace Common;

public static class Constants
{
    public static string BasePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "EasyImmoApp");
}