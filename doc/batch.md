# Batch Processing

A batch object is a wrapper for multiple [album](./album.md) [records](./record.md). It allows the [encoding](./encoding.md) of multiple videos into separate albums with their own songs.

Its [record](./record.md) file, too, reflects that:

```
some-album-record.txt  - album 1
/music/90's RB-016.txt - album 2
c:\\90's RB-019.txt    - album 3
|--------------------|
                     +-- album record file path
```