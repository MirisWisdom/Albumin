<html>
    <h1 align='center'>
        YouTube Album Extract
    </h1>
    <p align='center'>
        Extract & curate MP3 / FLAC songs from YouTube album videos.
        <br>
        <br>
        <img src='https://user-images.githubusercontent.com/10241434/135048812-156d9a9a-0218-42e8-9bcf-1b67ff7acbef.png'>
        <br>
        <img src='https://user-images.githubusercontent.com/10241434/135047939-dc7c2d36-a10c-4be2-ae0c-4961c3cb1a20.png'>
    </p>
</html>

## Introduction

This project allows you to transform long YouTube album videos into properly curated MP3 and FLAC songs.

## Features

- Encodes a long video's audio into separate MP3 and FLAC tracks
- Tagging abilities (title, album, artist(s), comment, genre, etc.)
- Downloading using YouTube-DL, or using an existing video file
- Embedded cover art, using thumbnails derived from the provided video using, or an existing image
- Batch processing of multiple album videos from YouTube or local videos

## Usage

| Parameter         | Description                                                                              |
| ----------------- | ---------------------------------------------------------------------------------------- |
| `--lossy`         | use lossy mp3 encoding instead of flac (and also jpeg instead of png for cover art)      |
| `--tracks=VALUE`  | path to records file with track numbers, timestamps and song titles                      |
| `--source=VALUE`  | path to the video containing the compiled songs (can be a youtube video or local file)   |
| `--batch=VALUE`   | encode (and download) albums specified in the given batch file                           |
| `--album=VALUE`   | album title to assign to the tracks' metadata; also, directory name to move tracks to    |
| `--artist=VALUE`  | album artist(s) to assign to the tracks' metadata; multiple: `--artist 'a' --artist 'b'` |
| `--genre=VALUE`   | genre to assign to the tracks' metadata                                                  |
| `--comment=VALUE` | comment to assign to the tracks' metadata                                                |
| `--cover=VALUE`   | optional path to album art image for assigning to songs                                  |

### Tracks Records

The records file specifies each song's track number, starting time in the video, and the title of the track.

For simplicity, create a text file containing the list of songs. Each line *must* comprise of the following attributes, in the given order:

1. Track number
2. Starting time in the provided video
3. Title of the track

Each attribute is separated by a space. Example of a valid file:

```
01 0:00:00 All You Need Is Love - 田村直美 「レイアース」OVA版主題歌
02 0:05:20 HEAVEN - HIM 「YAT安心！宇宙旅行」一期OP
03 0:08:48 僕であるために - FLYING KIDS 「逮捕しちゃうぞ」一期OP2
04 0:12:23 LOVE SOMEBODY - 福井麻利子 「逮捕しちゃうぞ」一期OP3
```

### Single Album

Create the records file as specified above, then invoke the program as necessary:

```shell
./gunloader \
    # download from youtube using youtube-dl
    --download "https://youtube.dl/id_goes_here" \
    # or use an existing video on your computer
    --source "~/video.mp4" \

    # file with track times & titles (see above)
    --tracks '~/tracks.txt' \

    # mass fill with metadata
    --album "A Nifty Title" \
    --genre "OP/ED/IN/IM" \
    --artist "Various" --artist "Artists" \
    --comment 'Very Important Music'
```

The program will compile the provided records file and metadata into a JSON file:

```json
{
  "video": "https://youtu.be/pCsFmYJh9sg",
  "title": "90s Nostalgia",
  "tracks": [
    {
      "title": "All You Need Is Love",
      "number": "01",
      "metadata": {
        "album": "90s Nostalgia",
        "cover": "",
        "genre": "OP / ED / IN / IM",
        "comment": "https://youtu.be/pCsFmYJh9sg",
        "artists": [
          "Various",
          "Artists"
        ]
      },
      "start": "0:00:00",
      "end": "0:05:20"
    },
    {
      "title": "Still Small Voice",
      "number": "03",
      "metadata": {
        "album": "90s Nostalgia",
        "cover": "",
        "genre": "OP / ED / IN / IM",
        "comment": "https://youtu.be/pCsFmYJh9sg",
        "artists": [
          "Various",
          "Artists"
        ]
      },
      "start": "0:05:20",
      "end": "0:09:37"
    }
}
```

Please review it and ensure that the values are your desired ones. The values in this file will be used for the encoding process.

### Batch Albums

Create a text file containing the list of albums. Each line *must* comprise of the following attributes, in the given order:

1. Album source (YouTube URL or a local file)
2. Path to the track records for the album (see above)
3. Title of the album (used for MP3/FLAC tagging & output directory)

Example of a valid batch file:

```
https://youtu.be/pCsFmYJh9sg 90s-songs.txt 90'sアニメ主題歌セレクション RB-018【奇跡の向こう側へ】 Ver.2
90s-nostalgic-songs.mp4 tracks-02.txt 90'sアニメ主題歌セレクション RB-019【傷だらけのツバサ】
```

Then, invoke the program with the `--batch <batch file>` argument:

```shell
./gunloader \
    --batch "90s-albums.txt" \

    # mass fill with metadata
    --genre "OP/ED/IN/IM" \
    --artist "Various"  --artist "Artists" \
    --comment 'Very Important Music'
```

Like the records file, the program will compile a more flexible JSON version of the batch file containing all of the albums.

## Dependencies

- NET Core 5 for running this poor man's solution
- FFmpeg for audio & cover art extraction
- LAME for MP3 encoding w/ metadata
- FLAC for FLAC encoding w/ metadata
- youtube-dl for downloading videos

## FAQ

It's called `gunloader` because it was originally meant to be used to download compilations by Gundober.

This repository's numbskull of an author realised only afterwards that this solution works for any YouTube album video.
