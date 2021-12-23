# Gunloader Usage

To keep things simple, you can just double click on Gunloader. The wizard will walk you through the whole process.

![wizard](https://user-images.githubusercontent.com/10241434/145333289-27e462f6-eb21-4793-853b-1b18b684994e.png)

If you are a Linux user, you may have to run `./gunloader` through the terminal to see the wizard!

## Album Records

The [records](./doc/album.md) file contains the main album information:

- The **first two lines** specify the **album title** and **video source**. The video source can be either a YouTube URL or a local video file.
- **Subsequent lines represent the tracks**. Each line *must* comprise of the following attributes, in the given order and separated by spaces:
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

## YouTube Chapters

If you want Gunloader to attempt to use a YouTube video's chapter information for the songs, please:

1. Ensure the YouTube video in question has **chapters in its timeline**; and
2. Create a [records](./doc/album.md) file containing just the **YouTube video URL**!

## Advanced Usage

For more advanced usage, you can pass parameters to Gunloader and skip the wizard altogether.

First, create the album records file as described [here](#album-records). Once you're done, invoke the program like so:

```shell
./gunloader \
    # records file
    --album '~/album.txt' \
    --album '~/another-album.txt' \

    # encoding of choice (default is mp3)
    --format flac \

    # losslessly split the original audio
    # no metadata or re-encoding is applied
    --format raw \

    # mass fill with optional metadata
    --genre "OP/ED/IN/IM" \
    --artist "Various" --artist "Artists" \
    --comment 'Very Important Music'
```

The program will first generate a [`.gun` file](./doc/compiling.md) describing the tracks. Feel free to review and edit each track's metadata and whatnot as necessary.

Once you're ready, continue with the program and it will start curating the video into separate songs.

#### Parameters

| Parameter         | Description                                                                                                 |
| ----------------- | ----------------------------------------------------------------------------------------------------------- |
| `--format=VALUE`  | audio encoding format; supported values: `mp3`, `flac`, `vorbis`, `opus`, `aac`, `raw`                      |
| `--album=VALUE`   | path to [album records](#album-records) file(s) (see below); multiple `--album 'abc.txt' --album 'xyz.txt'` |
| `--artist=VALUE`  | album artist(s) to assign to the tracks' metadata; multiple: `--artist 'a' --artist 'b'`                    |
| `--genre=VALUE`   | genre to assign to the tracks' metadata                                                                     |
| `--comment=VALUE` | comment to assign to the tracks' metadata                                                                   |
| `--cover=VALUE`   | optional path to album art image for assigning to songs                                                     |

Additional parameters can be found using `--help`.