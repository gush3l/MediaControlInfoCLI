using System.Text.Json;
using Windows.Media.Control;
using Windows.Storage.Streams;
using System.Drawing;
using System.Drawing.Imaging;

using NAudio.CoreAudioApi;

namespace MediaControlInfoCLI
{

    class MediaInfo
    {
        public required string AlbumArtist { get; set; }
        public required string AlbumTitle { get; set; }
        public int AlbumTrackCount { get; set; }
        public required string Artist { get; set; }
        public required string Genres { get; set; }
        public Windows.Media.MediaPlaybackType? PlaybackType { get; set; }
        public required string Subtitle { get; set; }
        public required string ThumbnailBase64 { get; set; }
        public required string Title { get; set; }
        public int TrackNumber { get; set; }
        public required string SessionOrigin { get; set; }
        public required string RepeatMode { get; set; }
        public required string ShuffleMode { get; set; }
        public int TrackDuration { get; set; }
        public int TrackProgress { get; set; }
        public required string PlaybackStatus { get; set; }
        public int Volume { get; set; }
        public bool IsMuted { get; set; }
    }

    class Program
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        private static string? jsonOutput;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (args.Length == 0)
            {
                jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "No commands used." }, JsonSerializerOptions);
                Console.WriteLine(jsonOutput);
                return;
            }

            var sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var currentSession = sessionManager.GetCurrentSession();

            if (currentSession == null)
            {
                jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "No media session found." }, JsonSerializerOptions);
                Console.WriteLine(jsonOutput);
                return;
            }

            switch (args[0])
            {
                case "info":
                    if (args.Length == 2 && args[1] == "thumbnail")
                    {
                        await MediaInfo(currentSession);
                    }
                    else
                    {
                        await MediaInfo(currentSession, false);
                    }
                    return;
                case "play":
                    await currentSession.TryPlayAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried playing current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "pause":
                    await currentSession.TryPauseAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried pausing current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "stop":
                    await currentSession.TryStopAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried stopping current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "next":
                    await currentSession.TrySkipNextAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried playing next track for current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "previous":
                    await currentSession.TrySkipPreviousAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried playing previous track for current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "fastforward":
                    await currentSession.TryFastForwardAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried fastforwarding the track for current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "rewind":
                    await currentSession.TryRewindAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried rewinding the track for current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "toggleplaypause":
                    await currentSession.TryTogglePlayPauseAsync();
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried play/pausing current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "repeatMode":
                    if (args.Length == 2)
                    {
                        if (args[1] == "none")
                        {
                            await currentSession.TryChangeAutoRepeatModeAsync(Windows.Media.MediaPlaybackAutoRepeatMode.None);
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried setting repeat mode to none for current session." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                        }
                        else if (args[1] == "track")
                        {
                            await currentSession.TryChangeAutoRepeatModeAsync(Windows.Media.MediaPlaybackAutoRepeatMode.Track);
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried setting repeat mode to track for current session." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                        }
                        else if (args[1] == "list")
                        {
                            await currentSession.TryChangeAutoRepeatModeAsync(Windows.Media.MediaPlaybackAutoRepeatMode.List);
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried setting repeat mode to list for current session." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                        }
                        else
                        {
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Invalid repeat mode. Can be none, track, or list." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                            return;
                        }
                    }
                    else
                    {
                        jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "No repeat mode specified." }, JsonSerializerOptions);
                        Console.WriteLine(jsonOutput);
                        return;
                    }
                    return;
                case "shuffleMode":
                    if (args.Length == 2)
                    {
                        if (args[1] == "true")
                        {
                            await currentSession.TryChangeShuffleActiveAsync(true);
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried setting shuffle mode to true for current session." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                        }
                        else if (args[1] == "false")
                        {
                            await currentSession.TryChangeShuffleActiveAsync(false);
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried setting shuffle mode to false for current session." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                        }
                        else
                        {
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Invalid shuffle mode. Set to true or false." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                            return;
                        }
                    }
                    else
                    {
                        jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "No shuffle mode specified." }, JsonSerializerOptions);
                        Console.WriteLine(jsonOutput);
                        return;
                    }
                    return;
                case "playbackPosition":
                    if (args.Length == 2)
                    {
                        if (long.TryParse(args[1], out long position))
                        {
                            // Convert milliseconds to ticks.
                            await currentSession.TryChangePlaybackPositionAsync(position * 10000);
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried setting playback position for current session." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                        }
                        else
                        {
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Invalid playback position. Must be a long number." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                            return;
                        }
                    }
                    else
                    {
                        jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "No playback position specified." }, JsonSerializerOptions);
                        Console.WriteLine(jsonOutput);
                        return;
                    }
                    return;
                case "volume":
                    if (args.Length == 2)
                    {
                        if (int.TryParse(args[1], out int volume))
                        {
                            if (volume >= 0 && volume <= 100)
                            {
                                new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioEndpointVolume.MasterVolumeLevelScalar = volume / 100f;
                                jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried setting volume for current session." }, JsonSerializerOptions);
                                Console.WriteLine(jsonOutput);
                            }
                            else
                            {
                                jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Invalid volume. Must be between 0 and 100." }, JsonSerializerOptions);
                                Console.WriteLine(jsonOutput);
                                return;
                            }
                        }
                        else
                        {
                            jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Invalid volume. Must be an integer." }, JsonSerializerOptions);
                            Console.WriteLine(jsonOutput);
                            return;
                        }
                    }
                    else
                    {
                        jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "No volume specified." }, JsonSerializerOptions);
                        Console.WriteLine(jsonOutput);
                        return;
                    }
                    return;
                case "mute":
                    new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioEndpointVolume.Mute = true;
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried muting current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                case "unmute":
                    new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioEndpointVolume.Mute = false;
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Success", Content = "Tried unmuting current session." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
                default:
                    jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Invalid command." }, JsonSerializerOptions);
                    Console.WriteLine(jsonOutput);
                    return;
            }
        }

        static async Task MediaInfo(GlobalSystemMediaTransportControlsSession currentSession, bool thumbnail = true)
        {
            var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();
            if (mediaProperties == null)
            {
                jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Failed to get media properties." }, JsonSerializerOptions);
                Console.WriteLine(jsonOutput);
                return;
            }

            var playBackInfo = currentSession.GetPlaybackInfo();
            if (playBackInfo == null)
            {
                jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Failed to get playback info." }, JsonSerializerOptions);
                Console.WriteLine(jsonOutput);
                return;
            }

            var timeLineInfo = currentSession.GetTimelineProperties();
            if (timeLineInfo == null)
            {
                jsonOutput = JsonSerializer.Serialize(new { Response = "Error", Content = "Failed to get timeline properties." }, JsonSerializerOptions);
                Console.WriteLine(jsonOutput);
                return;
            }

            var mediaInfo = new MediaInfo
            {
                AlbumArtist = mediaProperties.AlbumArtist,
                AlbumTitle = mediaProperties.AlbumTitle,
                AlbumTrackCount = mediaProperties.AlbumTrackCount,
                Artist = mediaProperties.Artist,
                Genres = string.Join(", ", mediaProperties.Genres),
                PlaybackType = mediaProperties.PlaybackType,
                Subtitle = mediaProperties.Subtitle,
                ThumbnailBase64 = "null",
                Title = mediaProperties.Title,
                TrackNumber = mediaProperties.TrackNumber,
                SessionOrigin = currentSession.SourceAppUserModelId,
                RepeatMode = playBackInfo.AutoRepeatMode switch
                {
                    Windows.Media.MediaPlaybackAutoRepeatMode.None => "none",
                    Windows.Media.MediaPlaybackAutoRepeatMode.Track => "track",
                    Windows.Media.MediaPlaybackAutoRepeatMode.List => "list",
                    _ => "unknown"
                },
                ShuffleMode = playBackInfo.IsShuffleActive == null ? "unknown" : playBackInfo.IsShuffleActive.Value switch
                {
                    true => "on",
                    false => "off"
                },
                TrackDuration = (int)timeLineInfo.EndTime.TotalMilliseconds,
                TrackProgress = (int)timeLineInfo.Position.TotalMilliseconds,
                PlaybackStatus = playBackInfo.PlaybackStatus switch
                {
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed => "closed",
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Opened => "opened",
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Changing => "changing",
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped => "stopped",
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing => "playing",
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused => "paused",
                    _ => "unknown"
                },
                Volume = (int)(new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioEndpointVolume.MasterVolumeLevelScalar * 100),
                IsMuted = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioEndpointVolume.Mute
            };

            if (thumbnail)
            {
                var thumbnailTask = GetThumbnailBase64(mediaProperties.Thumbnail);
                if (await Task.WhenAny(thumbnailTask, Task.Delay(2000)) != thumbnailTask)
                {
                    mediaInfo.ThumbnailBase64 = "null";
                }
                else
                {
                    if (thumbnailTask.Result != null)
                    {
                        mediaInfo.ThumbnailBase64 = thumbnailTask.Result;
                    }
                    else
                    {
                        mediaInfo.ThumbnailBase64 = "null";
                    }
                }
            }

            jsonOutput = JsonSerializer.Serialize(new { Response = "MediaInfo", Content = mediaInfo }, JsonSerializerOptions);
            Console.WriteLine(jsonOutput);
        }

        static async Task<string?> GetThumbnailBase64(IRandomAccessStreamReference thumbnailReference)
        {
            if (thumbnailReference == null)
            {
                return "null";
            }

            using var thumbnailStream = await thumbnailReference.OpenReadAsync();
            using var memoryStream = new MemoryStream();

            await thumbnailStream.AsStreamForRead().CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            using var bitmap = Bitmap.FromStream(new MemoryStream(imageBytes));

            if (!bitmap.RawFormat.Equals(ImageFormat.Png))
            {
                using var pngMemoryStream = new MemoryStream();
                bitmap.Save(pngMemoryStream, ImageFormat.Png);
                imageBytes = pngMemoryStream.ToArray();
            }

            return Convert.ToBase64String(imageBytes);
        }

    }
}
