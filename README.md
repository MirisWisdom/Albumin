<html>
    <h1 align='center'>
        Gunloader
    </h1>
    <p align='center'>
        Download, extract & curate songs from YouTube album videos in your favourite audio format.
        <br>
        <a href='https://github.com/yumiris/Gunloader/releases/latest'>Download Latest Release</a>
    </p>
</html>

## Introduction

This project allows you to transform long YouTube album videos into properly curated songs:

![screenshot of curated songs](https://user-images.githubusercontent.com/10241434/135047939-dc7c2d36-a10c-4be2-ae0c-4961c3cb1a20.png)

## Features

- Encodes a long video's audio into separate audio tracks
- Support for FLAC, MP3, Vorbis, Opus and AAC encoding formats
- Tagging abilities (title, album, artist(s), comment, genre, etc.)
- Downloading using YouTube-DL, or using an existing video file
- Embedded cover art, using thumbnails derived from the provided video, or using an existing image
- Batch processing of multiple album videos from YouTube or local videos
- Song information can be read from a file, or from a YouTube video's chapters/timeline
- Optional lossless splitting of the video's original audio, without any re-encoding whatsoever

## Usage

Simply double click on Gunloader, and the wizard will walk you through the whole process. It will generate a sample records file for you to edit, then ask you some additional quests regarding the encoding and metadata.

For further instructions, please refer to the [USAGE](./USAGE.md) document. Linux users may have to run `./gunloader` through the terminal to see the wizard.

## Installation

Download [the latest release from here](https://github.com/yumiris/Gunloader/releases/latest), and also make sure you have the dependencies you need installed. See below for further information!

## Dependencies

- [`youtube-dl`](https://ytdl-org.github.io/youtube-dl/) for downloading videos
- [`ffmpeg`](https://www.ffmpeg.org/) for audio & cover art extraction
- [`lame`](https://lame.sourceforge.net/) for MP3 encoding w/ metadata
- [`flac`](https://xiph.org/flac/) for FLAC encoding w/ metadata
- [`oggenc`](https://www.xiph.org/vorbis/) for Vorbis encoding w/ metadata
- [`opusenc`](https://wiki.xiph.org/Opus-tools) for Opus encoding w/ metadata
- [`ffmpeg`](https://www.ffmpeg.org/) for AAC encoding w/ metadata

If you need to specify a dependency's executable to the program, do it like so:

```shell
./gunloader \
    # records file
    --album '~/album.txt' \

    # paths to dependencies
    --ffmpeg "c:\\ffmpeg.exe" \
    --flac '/bin/flac' \
    --youtube-dl ~/ytdl \

    # mass fill with optional metadata
    --genre "OP/ED/IN/IM" \
    --artist "Various" --artist "Artists" \
    --comment 'Very Important Music'
```

## FAQ

It's called `gunloader` because it was originally meant to be used to download compilations by Gundober.

This repository's numbskull of an author realised only afterwards that this solution works for any YouTube album video.
