namespace SunamoIni;

/// <summary>
/// Create a New INI file to store or load data
/// </summary>
public class IniFile
{
    // TODO: Package was incompatible with netstandard, install new one and uncomment
    private readonly Configuration? configuration;

    /// <summary>
    /// Path to the INI file
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Initializes a new instance of the IniFile class
    /// </summary>
    /// <param name="iniPath">Path to the INI file</param>
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
    /// Write value to INI file using Win32 methods
    /// </summary>
    /// <param name="section">Section name</param>
    /// <param name="key">Key name</param>
    /// <param name="value">Value to write</param>
    public void IniWriteValue(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, Path);
    }

    /// <summary>
    /// Read value from INI file, always uses SharpConfig if available
    /// </summary>
    /// <param name="section">Section name</param>
    /// <param name="key">Key name</param>
    /// <returns>Value from INI file</returns>
    public string IniReadValue(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    /// <summary>
    /// Read value from INI file using SharpConfig
    /// </summary>
    /// <param name="section">Section name</param>
    /// <param name="key">Key name</param>
    /// <returns>Value from INI file</returns>
    public string IniReadValueSharpConfig(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    /// <summary>
    /// Read value from INI file with option to use SharpConfig or Win32 methods
    /// </summary>
    /// <param name="isUsingSharpConfig">Whether to use SharpConfig library</param>
    /// <param name="section">Section name</param>
    /// <param name="key">Key name</param>
    /// <returns>Value from INI file</returns>
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
    /// Creates an IniFile instance for the specified path
    /// </summary>
    /// <param name="iniFilePath">Path to the INI file</param>
    /// <returns>New IniFile instance</returns>
    public static IniFile InStartupPath(string iniFilePath)
    {
        // TODO: Package was incompatible with netstandard, install new one and uncomment
        return new IniFile(iniFilePath);
    }
}