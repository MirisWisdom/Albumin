# Gunloader Changelog

## [v0.7.5] - 2021-12-22

### AAC encoding support

This update introduces support for encoding audio to AAC (.m4a) files. Invoke `gunloader` with `--format aac` or `--format m4a` - or specify `aac` in the wizard -- to use this format.

#### Notes

- FFmpeg is used for this encoding procedure, with the native AAC encoder. This is to mitigate scenarios where `libfdk_aac` is not found.
- Coverart is not embedded into the AAC files because FFmpeg doesn't seem to do it properly at the time of this commit. The alternative would be to use an additional dependency for the cover art.

## [v0.7.4] - 2021-12-08

### Interactive Wizard

This update introduces an interactive wizard for Gunloader. When you run the program without specifying arguments, it will interactively walk you through setting everything up.

![image](https://user-images.githubusercontent.com/10241434/145281119-2c05acaf-c988-443c-a149-46a89550ae24.png)

## [v0.7.3] - 2021-12-04

### Publication Changes

- Official releases are now remarkably smaller
- 6.0 is now used instead of 5.0 for .NET Core

### Logic Tweaks & Improvements

- Fixed erroneous inference of songs from a given records file

## [v0.7.2] - 2021-10-25

### Conditional Audio Download

TLDR: Download *only* the audio when all tracks have local existing covers

Gunloader usually downloads both the video & audio from a YouTube URL, for the sake of using frames within the video as covers for each track.

When all tracks have local images assigned as covers to them, it's not necessary to download the video from YouTube; instead, we could just retrieve only the audio.

This release teaches Gunloader to download *only* the audio stream from YouTube when all of the Album Tracks have local images assigned to them.

### Logic Tweaks & Improvements

- Ensure no form of cover extraction is conducted during lossless cutting
- When the encoding is finished, the target directory's modification time will be set to the source file's modification time
- Use assigned cover if it exists locally & delete only the temporarily extracted covers

## [v0.7.1] - 2021-10-18

- Fixes a quirk where the YouTube Video URL wasn't assigned when using the chapters mode.
- More descriptive exceptions are thrown when sources are not found, for improved debugging.

## [v0.7.0] - 2021-10-14

### Lossless Audio Split w/o Re-Encoding

This version introduces the "raw" encoding mechanism, which does nothing except split the source audio into separate tracks without any encoding whatsoever. This option is perfect for those who prefer to simply split the audio into separate files in the original format.

To use, simply use `--format raw` as the parameter.

This feature - just like the YouTube chapter inference - is still in prototype stages and oddities might pop up. Don't hesitate to open an an issue if you encounter any quirks.

### Return to MP3 Encoding by Default

MP3 is once again the default format; however, `--format <format>` can continue to be used during invocation.

## [v0.6.0] - 2021-10-13

## YouTube Chapters/Timeline Support

Gunloader can now split songs using information from YouTube Chapters/Timeline metadata. This can spare you from manually writing out the songs and their starting times.

To enable this feature, make sure the records file you provide Gunloader has a **YouTube Video URL** as its **first line**. Of course, you will need to make sure the video in question has chapters!

## Quality of Life & Robustness

- Add ability to skip review prompting using `--no-prompt`;
- Trim Source, Album & Track Titles;
- Normalise Album Target path;
- Permit `;` and `#` comments in the album record file;

## [v0.5.0] - 2021-10-12

### Improved Album Record Files

Album record files can now have the album title & source video specified in them:

```
90'sアニメ主題歌セレクション RB-XYZ【奇跡の向こう側へ】 Ver.2
https://youtu.be/divcisums90

01 0:00:00 All You Need Is Love - 田村直美 「レイアース」OVA版主題歌
02 0:05:20 HEAVEN - HIM 「YAT安心！宇宙旅行」一期OP
03 0:08:48 僕であるために - FLYING KIDS 「逮捕しちゃうぞ」一期OP2
04 0:12:23 LOVE SOMEBODY - 福井麻利子 「逮捕しちゃうぞ」一期OP3
```

By virtue, `--album` is now used as an alias for `--records`/`--tracks`. Also, the `--title` and `--source` parameters have been dropped.

### Simplified Batch Mechanism

Processing multiple albums can now be done by simply passing the `--album` parameter multiple times:

```
./gunloader \
    --album '~/album 01 songs.txt' \
    --album '~/90s nostalgia album.txt' \
```

As such, batch files are no longer used.

## [v0.4.0] - 2021-10-12

### Support for Vorbis & Opus

Ogg Vorbis & Opus are now supported, using `oggenc` and `opusenc` respectively.

When invoking Gunloader, provide the `--format` (or `--encoder`) parameter with any of the following values: `mp3`, `flac`, `vorbis`, `opus`. By default, MP3 will be used.

### Dynamic `.gun` File Names

Dynamic names are used for the generated `.gun` files, instead of mere GUID files. For example:

- **Album `.gun` file**: `90'sアニメ主題歌セレクション RB-018【奇跡の向こう側へ】 Ver.2.gun`
- **Batch `.gun` file**: `2021-10-12 - 20 Albums.gun`

GUIDs will continue to be used as fallbacks when needed.

## [v0.3.1] - 2021-10-11

- Use `.gun` as extension instead of `.ser`, and pray for text editors to recognise it.
- Implement XML serialisation support, using `--xml` as the parameter.

## [v0.3.0] - 2021-10-10

The code base has been revamped in favour of a much more modular and robust architecture. The functionality is virtually the same; however, the structure of the JSON files has been revised. The files also now end in the more generic `.ser` (as in serialised) extension instead of `.json`.

## [v0.2.1] - 2021-10-06

Encode to lossless FLAC by default (instead of MP3)

## [v0.2.0] - 2021-10-06

Implement FLAC encoding support.

## [v0.1.1] - 2021-10-04

Fix issues caused by null values. One day, a testing suite may be developed.

## [v0.1.0] - 2021-10-03

Equivalent to [v0.0.2](https://github.com/MirisWisdom/YouTube.Album.Extract/releases/tag/v0.0.2), but with a more appropriate version bump.

- **Introduce new JSON format for records & batch files**
  - use records & batch files for metadata
- **Option to specify a local image for the album cover**, skipping the cover extraction procedure;
- **Ability to infer YouTube ID from local video name**. This can only be done if the file name:
  - is strictly 11 characters in length (excluding extension); and
  - contains only alphanumeric characters, dashes or underscores.
- Explicitly require a Records or Batch file upon invocation

## [v0.0.1] - 2021-09-30

It works as long as you strictly adhere to providing it valid arguments and data. A much more sensible architecture - along with robustness, defensiveness and other nifty goodies - will be implemented down the line.

### Features

- Encodes a long video's audio into separate MP3 tracks using FFmpeg
- Tagging abilities (title, album, artist(s), comment, genre, etc.) using LAME
- Downloading using YouTube-DL, or using an existing video file
- Embedded cover art, using thumbnails derived from the provided video
- Batch processing of multiple album videos from YouTube or local videos