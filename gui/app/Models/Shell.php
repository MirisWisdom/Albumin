<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Builder;

class Shell
{
    /**
     * @param Builder|Record $record
     * @return string
     */
    public static function generate(Builder|Record $record): string
    {
        $command = <<<SH
#!/bin/bash

#                       __                __
#    ____ ___  ______  / /___  ____ _____/ /__  _____
#   / __ `/ / / / __ \/ / __ \/ __ `/ __  / _ \/ ___/
#  / /_/ / /_/ / / / / / /_/ / /_/ / /_/ /  __/ /
#  \__, /\__,_/_/ /_/_/\____/\__,_/\__,_/\___/_/
# /____/
# ---------------------------------------------------
# Author :: Miris /// GitHub :: MirisWisdom/Albumin
# ---------------------------------------------------

# This simple Bash script lets you download the video
# using yt-dlp, and then takes care of extracting its
# audio into a file for each song w/ ffmpeg!

# It does not re-encode or tag any of the audio files
# due to raw preservation being its purpose.

# You should run it line by line rather than blindly!
# Feel free to tweak & suggest improvements!

# ---------------------------------------------------

# We'll be using the ID for both file and directory.
VIDEO_ID='$record->source_id'

# Create and enter the new directory.
mkdir -p "\${VIDEO_ID}" && cd "\${VIDEO_ID}"

# Download the audio file using yt-dlp.
yt-dlp -x -f bestaudio "\${VIDEO_ID}" -o "\${VIDEO_ID}.%%(ext)s"

# Figure out the downloaded file name and extension.
SRC_FILE=$(ls "\${VIDEO_ID}".*) # ls command should only output the downloaded file name!
FILE_EXT=\${SRC_FILE:12}        # file id + dot = 12 characters; extension length varies!


SH;

        foreach ($record->entries()->get() as $entry) {
            $command .= sprintf("ffmpeg -ss %s -to %s -i \"\${SRC_FILE}\" -vn -acodec copy \"%s.\${FILE_EXT}\"\n",
                                $entry->start,
                                $entry->end,
                                sprintf("%s. %s", str_pad($entry->number,
                                                          strlen(Entry::whereRecordId($record->id)
                                                                      ->orderByDesc('number')
                                                                      ->first()
                                                                     ->number),
                                                          0,
                                                          STR_PAD_LEFT),
                                        $entry->title)
            );
        }

        return $command;
    }
}
