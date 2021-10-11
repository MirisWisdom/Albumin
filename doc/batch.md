# Batch Processing

A batch object is a wrapper for multiple [album](./album.md) objects. It allows the encoding of multiple videos into separate albums with their own songs.

Its [record](./record.md) file, too, reflects that:

```
https://youtu.be/pCsFmYJh9sg 90s-songs.txt 90's RB-018 Ver.2 - album 1
90s-nostalgic-songs.mp4 tracks-02.txt 90' RB-019             - album 2
|---------------------| |-----------| |--------|
                      |             |          |
                      |             |          +-------------- album title
                      |             +------------------------- album records file
                      +--------------------------------------- album source video
```