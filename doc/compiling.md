# `.gun` File Compiling

## Introduction

Before Albumin [encodes](./encoding.md) the given video to music files, it first compiles the given [album record](./album.md) and [track metadata](./track.md) to a `.gun` file. The values in this file will be used during encoding, so it's recommended to review it! You have granular control over every single [track](./track.md)'s information.

This is what the file tends to look like:

```json
{
  "video": "https://youtu.be/pCsFmYJh9sg",
  "title": "90s Nostalgia",
  "tracks": [
    {
      "title": "All You Need Is Love",
      "number": "01",
      "metadata": {
        "album": "90s Nostalgia",
        "cover": "",
        "genre": "OP / ED / IN / IM",
        "comment": "https://youtu.be/pCsFmYJh9sg",
        "artists": [
          "Various",
          "Artists"
        ]
      },
      "start": "0:00:00",
      "end": "0:05:20"
    },
    {
      "title": "Still Small Voice",
      "number": "03",
      "metadata": {
        "album": "90s Nostalgia",
        "cover": "",
        "genre": "OP / ED / IN / IM",
        "comment": "https://youtu.be/pCsFmYJh9sg",
        "artists": [
          "Various",
          "Artists"
        ]
      },
      "start": "0:05:20",
      "end": "0:09:37"
    }
  ]
} 
```

## In-depth Pedantry

Compiling refers to serialising the [album](./album.md) object to a `.gun` file. Albumin first [hydrates](./hydration.md) the album object, then serialises the properties to the aforementioned `.gun` file, either in JSON or XML format.

The values in this file will be used for the encoding mechanism. Saving the data to a file provides advantages such as:

- Ability to review and edit the values with granularity over each [track](./track.md).
- Re-using the file down the line for repeating the [encoding](./encoding.md) procedure.
- Centralising the [track metadata](./track.md) and [album record](./album.md) values into one location.