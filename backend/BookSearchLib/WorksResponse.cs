using System.Text.Json.Serialization;

public class WorkResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("key")]
    public string Key { get; set; } = "";

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("covers")]
    public List<int>? Covers { get; set; }

    [JsonPropertyName("subject_places")]
    public List<string>? SubjectPlaces { get; set; }

    [JsonPropertyName("subjects")]
    public List<string>? Subjects { get; set; }

    [JsonPropertyName("subject_people")]
    public List<string>? SubjectPeople { get; set; }

    [JsonPropertyName("subject_times")]
    public List<string>? SubjectTimes { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("latest_revision")]
    public int LatestRevision { get; set; }

    [JsonPropertyName("revision")]
    public int Revision { get; set; }

    [JsonPropertyName("created")]
    public TimestampInfo? Created { get; set; }

    [JsonPropertyName("last_modified")]
    public TimestampInfo? LastModified { get; set; }

    [JsonPropertyName("authors")]
    public List<WorkAuthorEntry>? Authors { get; set; }

    [JsonPropertyName("type")]
    public WorkTypeInfo? Type { get; set; }
}

public class TimestampInfo
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

public class WorkAuthorEntry
{
    [JsonPropertyName("author")]
    public AuthorRef? Author { get; set; }

    [JsonPropertyName("type")]
    public WorkTypeInfo? Type { get; set; }
}

public class AuthorRef
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }
}

public class WorkTypeInfo
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }
}