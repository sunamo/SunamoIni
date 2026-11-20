namespace SunamoIni;

/// <summary>
/// Represents the ini file type.
/// </summary>
public class IniFile
{
    // TODO: Package was incompatible with netstandard, install new one and uncomment
    private readonly Configuration? configuration;

    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Ini file.
    /// </summary>
    /// <param name="iniPath">The ini path.</param>
    /// <returns>The ini file result.</returns>
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

    /// <summary>
    /// Ini write value.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The ini write value result.</returns>
    public void IniWriteValue(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, Path);
    }

    /// <summary>
    /// Initializes a new instance of the ini read value class.
    /// </summary>
    /// <param name="section">The section.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="Path">The path.</param>
    public string IniReadValue(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    /// <summary>
    /// Ini read value sharp config.
    /// </summary>
    /// <param name="true">The true.</param>
    /// <param name="section">The section.</param>
    /// <param name="key">The key.</param>
    /// <returns>The ini read value sharp config result.</returns>
    public string IniReadValueSharpConfig(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    /// <summary>
    /// Ini read value.
    /// </summary>
    /// <param name="true">The true.</param>
    /// <param name="section">The section.</param>
    /// <param name="key">The key.</param>
    /// <returns>The ini read value result.</returns>
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

    /// <summary>
    /// In startup path.
    /// </summary>
    /// <returns>The in startup path result.</returns>
    public static IniFile InStartupPath(string iniFilePath)
    {
        // TODO: Package was incompatible with netstandard, install new one and uncomment
        return new IniFile(iniFilePath);
    }
}
