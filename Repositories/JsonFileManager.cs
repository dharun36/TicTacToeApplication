using System.Text.Json;
using System.Text.Json.Serialization;

namespace TicTacToeApplication.Repositories;

/// <summary>
/// Contains the Write and load logics that writes and loads the file in json format.
/// </summary>
public class JsonFileManager
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    /// <summary>
    /// Writes all the data to the file.
    /// </summary>
    /// <typeparam name="T">The type parameter of the data object to be written.</typeparam>
    /// <param name="filePath">The path of the file where the data are stored.</param>
    /// <param name="items">List of the data that are to be added.</param>
    public void WriteAll<T>(string filePath, List<T> items)
    {
        string fullPath = Path.GetFullPath(filePath);
        string? directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string temporaryPath = fullPath + ".tmp";
        string json = JsonSerializer.Serialize(items, _options);

        try
        {
            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, fullPath, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    /// <summary>
    /// Loads all the content and loads into the file.
    /// </summary>
    /// <typeparam name="T">The type parameter of the data to read from the file.</typeparam>
    /// <param name="filePath">Path of the file from which the contents are loaded. </param>
    /// <returns>A list of data that are stored in the file.</returns>
    public List<T> LoadAll<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        try
        {
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }

            return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"The JSON data in '{filePath}' is invalid.", exception);
        }
        catch (IOException exception)
        {
            throw new IOException($"Unable to read '{filePath}'.", exception);
        }
    }
}
