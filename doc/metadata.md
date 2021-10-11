# Track Metadata

Metadata refers to the additional information which is embedded into an encoded audio file. Gunloader, at the moment, supports the following tags:

- Album title
- Cover art
- Genre
- Comment
- Artists

Metadata can be mass assigned through parameters when invoking Gunloader. Each individual track's metadata can be granularly edited in the [compiled `.gun` file](./compiling.md) before the [encoding](./encoding.md) process.

## Comment

If a [track](./track.md) has a blank comment, Gunloader will attempt to assign the YouTube video URL as the comment. This can only be possible if:

A. The source is a YouTube URL; or
B. The local file has a YouTube ID as its name

For the local file name to be acknowledged as a YouTube ID, it must be 11 characters long, with only alphanumeric/dashes/underscores being used.