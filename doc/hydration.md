# Hydration Process

In wacky Gunloader lingo, hydration refers to assigning [album](./album.md) property values, using data from the given [record](./record.md) and [metadata](./metadata.md) objects:

![hydration diagram](./hydration.png)

Hydration is a part of the [compiling](./compiling.md) procedure. [Batch](./batch.md) hydration is merely a wrapper around the album hydration process: it hydrates each album found in the respective batch file.