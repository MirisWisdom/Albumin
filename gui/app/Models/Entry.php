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

    public function record(): BelongsTo
    {
        return $this->belongsTo(Record::class);
    }
}
