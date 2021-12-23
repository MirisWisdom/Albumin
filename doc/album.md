# Album

An album represents a YouTube (or local) video, along with the [tracks](./track.md) inside it. When [encoding](./encoding.md) an album, each track will be extracted from the respective video, and then encoded into the desired format.

## Gunloader Records

Record files are plain text files for you to create and fill out with the bare minimum information that Gunloader needs:

- album title
- video source -- either a YouTube video or a local file
- [tracks](./track.md)' numbers, starting times and titles.

```
90's RB-XYZ Ver.2               - title
https://youtu.be/divcisums90    - source

01 0:00:00 All You Need Is Love - track 1
02 0:05:20 HEAVEN - HIM         - track 2
03 0:08:48 FLYING KIDS          - track 3
04 0:12:23 LOVE SOMEBODY        - track 4
 | |-----| |-----------|
 |       |             |
 |       |             +--------- track title
 |       +----------------------- starting time
 +------------------------------- track number
```

## YouTube Chapters

A special mode has been introduced in v0.6.0, where Gunloader can attempt to retrieve chapter metadata from YouTube for each Track's Title, Starting Time and Ending Time.

![chapters](./chapters.svg)

To activate this mode, the given records file should simply contain the **YouTube Video URL**. Of course, the YouTube video in question must contain chapters.