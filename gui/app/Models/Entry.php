<?php

namespace App\Models;

use Barryvdh\LaravelIdeHelper\Eloquent;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Database\Query\Builder;
use Illuminate\Http\Request;
use Illuminate\Support\Carbon;

/**
 * App\Models\Entry
 *
 * @property int $id
 * @property int $number
 * @property string $title
 * @property string $start
 * @property string $end
 * @property string|null $album
 * @property string|null $genre
 * @property array|null $artists
 * @property string $identifier
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property Carbon|null $deleted_at
 * @property int $record_id
 * @property-read Record|null $record
 * @method static Builder|Entry newModelQuery()
 * @method static Builder|Entry newQuery()
 * @method static Builder|Entry onlyTrashed()
 * @method static Builder|Entry query()
 * @method static Builder|Entry whereAlbum($value)
 * @method static Builder|Entry whereArtists($value)
 * @method static Builder|Entry whereCreatedAt($value)
 * @method static Builder|Entry whereDeletedAt($value)
 * @method static Builder|Entry whereEnd($value)
 * @method static Builder|Entry whereGenre($value)
 * @method static Builder|Entry whereId($value)
 * @method static Builder|Entry whereIdentifier($value)
 * @method static Builder|Entry whereNumber($value)
 * @method static Builder|Entry whereRecordId($value)
 * @method static Builder|Entry whereStart($value)
 * @method static Builder|Entry whereTitle($value)
 * @method static Builder|Entry whereUpdatedAt($value)
 * @method static Builder|Entry withTrashed()
 * @method static Builder|Entry withoutTrashed()
 * @mixin Eloquent
 */
class Entry extends Model
{
    use HasFactory, SoftDeletes;

    protected $casts = [
        'artists' => 'array'
    ];

    /**
     * Store a newly created resource in storage.
     *
     * @param Source $source
     * @param Record $record
     * @param Request $request
     * @return Entry
     */
    public static function store(Source $source, Record $record, Request $request): Entry
    {
        $entry             = new Entry();
        $entry->number     = self::normalise($request->input('number'));
        $entry->title      = self::normalise($request->input('title'));
        $entry->start      = self::normalise($request->input('start'));
        $entry->end        = self::normalise($request->input('end'));
        $entry->album      = self::normalise($request->input('album'));
        $entry->genre      = self::normalise($request->input('genre'));
        $entry->artists    = explode(';', trim($request->input('artists')));
        $entry->identifier = Identifier::infer();
        $entry->record_id  = $record->id;
        $entry->save();

        info('Registered new Entry to the database.', [
            'entry'  => $entry->id,
            'record' => $record->id,
            'source' => $source->id
        ]);

        return $entry;
    }

    private static function normalise(string $input): string
    {
        return !empty($input) ? trim($input) : $input;
    }

    public static function storeFromChapters(Record $record)
    {
        $number = 1;
        if ($record->source->chapters()->count() > 0) {
            foreach ($record->source->chapters()->get() as $chapter) {
                $entry             = new Entry();
                $entry->number     = $number;
                $entry->title      = $chapter->title;
                $entry->start      = $chapter->start;
                $entry->end        = $chapter->end;
                $entry->identifier = Identifier::infer();
                $entry->record_id  = $record->id;
                $entry->save();

                $number++;

                info('Registered new Entry from Source Chapter to the database.', [
                    'entry'   => $entry->id,
                    'record'  => $record->id,
                    'source'  => $record->source_id,
                    'chapter' => $chapter->id
                ]);
            }
        }
    }

    public static function batch(Source $source, Record $record, Request $request)
    {
        $separator = "\r\n";
        $line      = strtok($request->input('import'), $separator);

        while ($line !== false) {
            $arr = explode('|', $line);

            $entry             = new Entry();
            $entry->number     = self::normalise($arr[0]);
            $entry->title      = self::normalise($arr[1]);
            $entry->start      = self::normalise($arr[2]);
            $entry->end        = self::normalise($arr[3]);
            $entry->album      = isset($arr[4]) ? self::normalise($arr[4]) : null;
            $entry->genre      = isset($arr[5]) ? self::normalise($arr[5]) : null;
            $entry->artists    = isset($arr[6]) ? explode(';', trim($arr[6])) : null;
            $entry->identifier = Identifier::infer();
            $entry->record_id  = $record->id;
            $entry->save();

            info('Registered new Entry from batch request to the database.', [
                'entry'  => $entry->id,
                'record' => $record->id,
                'source' => $source->id
            ]);

            $line = strtok($separator);
        }
    }

    public function record(): BelongsTo
    {
        return $this->belongsTo(Record::class);
    }
}
