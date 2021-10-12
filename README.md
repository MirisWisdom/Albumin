<html>
    <h1 align='center'>
        YouTube Album Extract
    </h1>
    <p align='center'>
        Extract & curate songs from YouTube album videos in your favourite audio format.
        <br>
        <br>
        <img src='https://user-images.githubusercontent.com/10241434/135048812-156d9a9a-0218-42e8-9bcf-1b67ff7acbef.png'>
        <br>
        <img src='https://user-images.githubusercontent.com/10241434/135047939-dc7c2d36-a10c-4be2-ae0c-4961c3cb1a20.png'>
    </p>
</html>

## Introduction

This project allows you to transform long YouTube album videos into properly curated songs, with support for popular audio formats.

## Features

- Encodes a long video's audio into separate audio tracks
- Support for FLAC, MP3, Vorbis and Opus encoding formats
- Tagging abilities (title, album, artist(s), comment, genre, etc.)
- Downloading using YouTube-DL, or using an existing video file
- Embedded cover art, using thumbnails derived from the provided video, or using an existing image
- Batch processing of multiple album videos from YouTube or local videos

## Album Records

The [records](./doc/album.md) file contains the main album information:

- The first two lines specify the album title and video source. The video source can be either a YouTube URL or a local video file.
- Subsequent lines represent the tracks. Each *must* comprise of the following attributes, in the given order and separated by spaces:
  1. Track number
  2. Starting time in the provided video
  3. Title of the track

```
90'sアニメ主題歌セレクション RB-XYZ【奇跡の向こう側へ】 Ver.2
https://youtu.be/divcisums90

01 0:00:00 All You Need Is Love - 田村直美 「レイアース」OVA版主題歌
02 0:05:20 HEAVEN - HIM 「YAT安心！宇宙旅行」一期OP
03 0:08:48 僕であるために - FLYING KIDS 「逮捕しちゃうぞ」一期OP2
04 0:12:23 LOVE SOMEBODY - 福井麻利子 「逮捕しちゃうぞ」一期OP3
```

## Usage

| Parameter         | Description                                                                              |
| ----------------- | ---------------------------------------------------------------------------------------- |
| `--format=VALUE`  | audio encoding format; supported values: `mp3`, `flac`, `vorbis`, `opus`                 |
| `--album=VALUE`   | path to album record file(s) (see above); multiple `--album 'abc.txt' --album 'xyz.txt'` |
| `--artist=VALUE`  | album artist(s) to assign to the tracks' metadata; multiple: `--artist 'a' --artist 'b'` |
| `--genre=VALUE`   | genre to assign to the tracks' metadata                                                  |
| `--comment=VALUE` | comment to assign to the tracks' metadata                                                |
| `--cover=VALUE`   | optional path to album art image for assigning to songs                                  |
| `--xml`           | use xml format instead of json                                                           |

Create the records file as described above, then invoke the program:

```shell
./gunloader \
    --album '~/album.txt' \

    # mass fill with optional metadata
    --genre "OP/ED/IN/IM" \
    --artist "Various" --artist "Artists" \
    --comment 'Very Important Music'
```

The program will compile the provided records file and metadata into a [`.gun` file](./doc/compiling.md). Please review it and ensure that the values are your desired ones. The values in this file will be used for the encoding process.

Also, XML can be used instead of JSON by passing `--xml` as a parameter.

## Dependencies

- [`youtube-dl`](https://ytdl-org.github.io/youtube-dl/) for downloading videos
- [`ffmpeg`](https://www.ffmpeg.org/) for audio & cover art extraction
- [`lame`](https://lame.sourceforge.net/) for MP3 encoding w/ metadata
- [`flac`](https://xiph.org/flac/) for FLAC encoding w/ metadata
- [`oggenc`](https://www.xiph.org/vorbis/) for Vorbis encoding w/ metadata
- [`opusenc`](https://wiki.xiph.org/Opus-tools) for Opus encoding w/ metadata

## FAQ

It's called `gunloader` because it was originally meant to be used to download compilations by Gundober.

This repository's numbskull of an author realised only afterwards that this solution works for any YouTube album video.
