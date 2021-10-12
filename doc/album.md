# Album

An album represents a YouTube (or local) video, along with the [tracks](./track.md) inside it. When [encoding](./encoding.md) an album, each Track will be extracted from the respective video, and then encoded into the desired format.

The [records](./record.md) file represents the title, sourcec video and [tracks](./track.md) that belong to the respective album:

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