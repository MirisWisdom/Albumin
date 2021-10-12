# Encoding Process

The encoding process creates an audio file of a [track](./track.md) using the chosen format (FLAC by default). It encodes its [intermediate audio file](./intermediate.md) along with the [cover art](./cover.md) and any given [metadata](./metadata.md):

![encoding diagram](./encoding.png)

| Format     | Dependency |
| ---------- | ---------- |
| MP3        | `lame`     |
| FLAC       | `flac`     |
| Ogg Vorbis | `oggenc`   |
| Ogg Opus   | `opusenc`  |
