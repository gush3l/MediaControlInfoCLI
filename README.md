# MediaControlInfoCLI

MediaControlInfoCLI is a command-line interface (CLI) tool for controlling and retrieving information about the current media session on Windows. It leverages the Windows Media Control API and NAudio library to provide various media control functionalities.

## Features

- Retrieve media information including album artist, album title, artist, genres, playback type, subtitle, thumbnail, title, track number, session origin, repeat mode, shuffle mode, track duration, track progress, playback status, volume, and mute status.
- Control media playback with commands such as play, pause, stop, next, previous, fast forward, rewind, toggle play/pause, change repeat mode, change shuffle mode, set playback position, set volume, mute, and unmute.

## Usage

To use MediaControlInfoCLI, run the executable with the desired command:

`.\MediaControlInfoCLI.exe [command]`

### Commands

- `info [thumbnail]`: Retrieve media information. If `thumbnail` is specified, include the thumbnail in the output.
- `play`: Play the current media session.
- `pause`: Pause the current media session.
- `stop`: Stop the current media session.
- `next`: Skip to the next track in the current media session.
- `previous`: Skip to the previous track in the current media session.
- `fastforward`: Fast forward the current track.
- `rewind`: Rewind the current track.
- `toggleplaypause`: Toggle between play and pause for the current media session.
- `repeatMode <none|track|list>`: Set the repeat mode for the current media session.
- `shuffleMode <true|false>`: Set the shuffle mode for the current media session.
- `playbackPosition <position>`: Set the playback position for the current media session (in milliseconds).
- `volume <0-100>`: Set the volume for the current media session.
- `mute`: Mute the current media session.
- `unmute`: Unmute the current media session.

## Building

To build MediaControlInfoCLI, you need to have .NET 8 installed. You need to `Publish` the project to create a self-contained executable.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
