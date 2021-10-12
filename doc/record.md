# Record Files

Record files are plain text files for you to create and fill out with the bare minimum information that Gunloader needs, i.e.

- the [tracks](./track.md) in an [album](./album.md)
- the [albums](./album.md) in a [batch](./batch.md)

They are meant to be simple and straightforward to edit.

## Album Records

Album records list out the [tracks](./track.md)' numbers, starting times, and titles:

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

## Batch Records

Batch records list out the [albums](./album.md)' records files:

```
some-album-record.txt  - album 1
/music/90's RB-016.txt - album 2
c:\\90's RB-019.txt    - album 3
|-----------------|
                  +----- album record file path
```