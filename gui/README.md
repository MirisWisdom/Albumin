<html>
    <h1 align='center'>
        Gunloader Web GUI
    </h1>
    <p align='center'>
        Create and find instructions for Gunloader to download YouTube album videos into separate songs.
    </p>
    <p align='center'>
        <a href='https://gunloader.miris.design'>Gunloader Web GUI</a>
        •
        <a href='https://github.com/MirisWisdom/Gunloader/releases/latest'>Download Gunloader CLI</a>
    </p>
</html>

![gunloader web gui screenshot](https://user-images.githubusercontent.com/10241434/174534383-2a144ef9-5a00-4348-8328-b7d05b8461f5.png)

This project lets you generate instructions for the Gunloader CLI to download and split up videos into separate audio track files. It is a simple companion to the main CLI app, with the intent of being accessible from any device.

Long, hours-long video compilations without existing track times are a challenge to split up. As such, this website allows anyone to optionally publish the track times for a video and letting others benefit from the effort.

# Usage

1. Go to the Web UI, and provide a YouTube URL.
2. If somebody already created a record, you can copy the ID for it.
3. Otherwise, create a new record with song information and timings.
4. Run the [Gunloader CLI](https://github.com/MirisWisdom/Gunloader/) and provide the ID shown in the Web GUI.

## Dependencies

Please refer to the main [Gunloader CLI page](https://github.com/MirisWisdom/Gunloader/) for further information.

In essence:

- [`yt-dlp`](https://github.com/yt-dlp/yt-dlp) for downloading videos (`youtube-dl` can also be used)
- [`ffmpeg`](https://www.ffmpeg.org/) for audio & cover art extraction
- [`lame`](https://lame.sourceforge.net/) for MP3 encoding w/ metadata
- [`flac`](https://xiph.org/flac/) for FLAC encoding w/ metadata
- [`oggenc`](https://www.xiph.org/vorbis/) for Vorbis encoding w/ metadata
- [`opusenc`](https://wiki.xiph.org/Opus-tools) for Opus encoding w/ metadata
- [`ffmpeg`](https://www.ffmpeg.org/) for AAC encoding w/ metadata
