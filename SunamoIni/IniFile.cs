namespace SunamoIni;

public class IniFile
{
    // TODO: Package was incompatible with netstandard, install new one and uncomment
    private readonly Configuration? configuration;

    public string Path { get; set; }

    public IniFile(string iniPath)
    {
        Path = iniPath;
        try
        {
            configuration = Configuration.LoadFromFile(Path);
        }
        catch (Exception)
        {
            // Configuration file could not be loaded, will use Win32 methods as fallback
        }
    }

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section,
        string key, string value, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section,
        string key, string defaultValue, StringBuilder returnValue,
        int size, string filePath);

    public void IniWriteValue(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, Path);
    }

    public string IniReadValue(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    public string IniReadValueSharpConfig(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    public string IniReadValue(bool isUsingSharpConfig, string section, string key)
    {
        if (isUsingSharpConfig)
        {
            // TODO: Package was incompatible with netstandard, install new one and uncomment
            if (configuration != null) return configuration[section][key].StringValue;
            return "";
        }
        var stringBuilder = new StringBuilder(255);
        GetPrivateProfileString(section, key, "", stringBuilder, int.MaxValue, Path);
        return stringBuilder.ToString();
    }

    public static IniFile InStartupPath(string iniFilePath)
    {
        // TODO: Package was incompatible with netstandard, install new one and uncomment
        return new IniFile(iniFilePath);
    }
}
