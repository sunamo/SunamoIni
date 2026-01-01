namespace SunamoIni;

/// <summary>
/// EN: Create a New INI file to store or load data
/// CZ: Vytvoří nový INI soubor pro ukládání nebo načítání dat
/// </summary>
public class IniFile
{
    // TODO: Package was incompatible with netstandard, install new one and uncomment
    private readonly Configuration? configuration;

    /// <summary>
    /// EN: Path to the INI file
    /// CZ: Cesta k INI souboru
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// EN: Initializes a new instance of the IniFile class
    /// CZ: Inicializuje novou instanci třídy IniFile
    /// </summary>
    /// <param name="iniPath">EN: Path to the INI file / CZ: Cesta k INI souboru</param>
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
        string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section,
        string key, string def, StringBuilder retVal,
        int size, string filePath);

    /// <summary>
    /// EN: Write value to INI file using Win32 methods
    /// CZ: Zapíše hodnotu do INI souboru pomocí Win32 metod
    /// </summary>
    /// <param name="section">EN: Section name / CZ: Název sekce</param>
    /// <param name="key">EN: Key name / CZ: Název klíče</param>
    /// <param name="value">EN: Value to write / CZ: Hodnota k zapsání</param>
    public void IniWriteValue(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, Path);
    }

    /// <summary>
    /// EN: Read value from INI file, always uses SharpConfig if available
    /// CZ: Načte hodnotu z INI souboru, vždy používá SharpConfig pokud je dostupný
    /// </summary>
    /// <param name="section">EN: Section name / CZ: Název sekce</param>
    /// <param name="key">EN: Key name / CZ: Název klíče</param>
    /// <returns>EN: Value from INI file / CZ: Hodnota z INI souboru</returns>
    public string IniReadValue(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    /// <summary>
    /// EN: Read value from INI file using SharpConfig
    /// CZ: Načte hodnotu z INI souboru pomocí SharpConfig
    /// </summary>
    /// <param name="section">EN: Section name / CZ: Název sekce</param>
    /// <param name="key">EN: Key name / CZ: Název klíče</param>
    /// <returns>EN: Value from INI file / CZ: Hodnota z INI souboru</returns>
    public string IniReadValueSharpConfig(string section, string key)
    {
        return IniReadValue(true, section, key);
    }

    /// <summary>
    /// EN: Read value from INI file with option to use SharpConfig or Win32 methods
    /// CZ: Načte hodnotu z INI souboru s možností použít SharpConfig nebo Win32 metody
    /// </summary>
    /// <param name="useSharpConfig">EN: Whether to use SharpConfig library / CZ: Zda použít SharpConfig knihovnu</param>
    /// <param name="section">EN: Section name / CZ: Název sekce</param>
    /// <param name="key">EN: Key name / CZ: Název klíče</param>
    /// <returns>EN: Value from INI file / CZ: Hodnota z INI souboru</returns>
    public string IniReadValue(bool useSharpConfig, string section, string key)
    {
        if (useSharpConfig)
        {
            // TODO: Package was incompatible with netstandard, install new one and uncomment
            if (configuration != null) return configuration[section][key].StringValue;
            return "";
        }
        var result = new StringBuilder(255);
        GetPrivateProfileString(section, key, "", result, int.MaxValue, Path);
        return result.ToString();
    }

    /// <summary>
    /// EN: Creates an IniFile instance for the specified path
    /// CZ: Vytvoří instanci IniFile pro zadanou cestu
    /// </summary>
    /// <param name="iniFilePath">EN: Path to the INI file / CZ: Cesta k INI souboru</param>
    /// <returns>EN: New IniFile instance / CZ: Nová instance IniFile</returns>
    public static IniFile InStartupPath(string iniFilePath)
    {
        // TODO: Package was incompatible with netstandard, install new one and uncomment
        return new IniFile(iniFilePath);
    }
}