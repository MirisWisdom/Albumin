# Track

A song within an [album](./album.md) that is:

1. [extracted](./intermediate.md) from a YouTube or local video; and
2. [encoded](./encoding.md) into a MP3 or FLAC file; and
3. contains [metadata](./metadata.md) and a [cover art](./cover.md).

On top of the metadata, a track represents its title, track number in the album, and starting + ending times.

## Ending Time

The ending time of a track is the *subsequent track's* **starting time**:

```
01 0:00:00 All You Need Is Love - track 1
   |-----|
         +----------------------- STARTING TIME FOR TRACK 1

02 0:12:23 LOVE SOMEBODY        - track 2
   |-----|
         +----------------------- STARTING TIME FOR TRACK 2, ENDING TIME FOR TRACK 1
```

An ending time can be explicitly assigned in the [compiled `.gun` file](./compiling.md) before the [encoding](./encoding.md) process.

## File Name

The encoded track name adheres to the "No. Title.ext" extension, e.g.:

- `01. All You Need Is Love - 田村直美 「レイアース」OVA版主題歌.mp3`
- `05. 僕であるために - FLYING KIDS 「逮捕しちゃうぞ」一期OP2.flac`

The title will be normalised before being assigned as the file name. Normalisation is simply the removal of the following characters:

- `<`
- `>`
- `:`
- `\`
- `/`
- `\`
- `|`
- `?`
- `*`

This ensures that the files are safe to keep on Windows and Linux filesystems.