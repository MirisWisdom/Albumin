# Encoding Process

The encoding process creates an MP3 or FLAC of a [track](./track.md). It encodes its [intermediate audio file](./intermediate.md) along with the [cover art](./cover.md) and any given [metadata](./metadata.md):

![encoding diagram](./encoding.png)

- For MP3, `lame` is used.
- For FLAC, `flac` is used.