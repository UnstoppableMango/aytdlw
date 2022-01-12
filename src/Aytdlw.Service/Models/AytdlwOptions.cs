namespace Aytdlw.Service.Models;

public record AytdlwOptions
{
    public bool? AddMetadata { get; init; }
    
    public bool? AudioMultiStreams { get; init; }
    
    public string? Cookies { get; init; }
    
    public bool? EmbedSubs { get; init; }
    
    public bool? EmbedThumbnail { get; init; }
    
    public string? Format { get; init; }
    
    public string? MergeOutputFormat { get; init; }
    
    public string? Output { get; init; }
    
    public string? Password { get; init; }
    
    public string? Username { get; init; }
}
