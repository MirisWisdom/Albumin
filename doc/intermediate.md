# Intermediate Audio File

Albumin will use FFmpeg to extract a WAV from the source video at the [track](./track.md)'s start and end values. This file then gets encoded with the cover art and [track metadata](./track.md).

The use of a intermediate WAV file is mostly for pragmatic reasons: it's a fast and simple way of getting a lossless audio file that the [encoding](./encoding.md) programs can encode without any problems.