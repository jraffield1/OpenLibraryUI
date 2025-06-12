using System.Text.Json;
using System.Text.Json.Serialization;

public class WorkResponse
{
    public string Title { get; set; } = "";
    public string Key { get; set; } = "";
    [JsonConverter(typeof(DescriptionFieldConverter))]
    public string? Description { get; set; }
    public List<int>? Covers { get; set; }
    public List<string>? SubjectPlaces { get; set; }
    public List<string>? Subjects { get; set; }
    public List<string>? SubjectPeople { get; set; }
    public List<string>? SubjectTimes { get; set; }
    public string? Location { get; set; }
    public int LatestRevision { get; set; }
    public int Revision { get; set; }
    public TimestampInfo? Created { get; set; }
    public TimestampInfo? LastModified { get; set; }
    public List<WorkAuthorEntry>? Authors { get; set; }
    public WorkTypeInfo? Type { get; set; }
}

public class TimestampInfo
{
    public string? Type { get; set; }
    public string? Value { get; set; }
}

public class WorkAuthorEntry
{
    public AuthorRef? Author { get; set; }
    public WorkTypeInfo? Type { get; set; }
}

public class AuthorRef
{
    public string? Key { get; set; }
}

public class WorkTypeInfo
{
    public string? Key { get; set; }
}

public class DescriptionFieldConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            if (doc.RootElement.TryGetProperty("value", out var valueProp) &&
                valueProp.ValueKind == JsonValueKind.String)
            {
                return valueProp.GetString();
            }
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}