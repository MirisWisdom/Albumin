<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Builder;

class Instruction
{
    public static function generate(Record|Builder $record)
    {
        $tracks = [];

        foreach ($record->entries()->get() as $entry) {
            $tracks[] = [
                'number'   => $entry->number,
                'title'    => $entry->title,
                'start'    => $entry->start,
                'end'      => $entry->end,
                'metadata' => [
                    'cover'   => '',
                    'album'   => $entry->album,
                    'genre'   => $entry->genre,
                    'comment' => 'https://youtu.be/' . $record->source_id,
                    'artists' => $entry->artists,
                ]
            ];
        }

        return [
            'video'  => sprintf("https://youtu.be/%s", $record->source_id),
            'title'  => 'Songs from ' . $record->source_id,
            'tracks' => $tracks
        ];
    }
}
