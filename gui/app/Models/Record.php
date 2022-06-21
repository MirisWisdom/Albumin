<?php

namespace App\Models;

use Barryvdh\LaravelIdeHelper\Eloquent;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Collection;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Support\Carbon;

/**
 * App\Models\Record
 *
 * @property int $id
 * @property string $alias
 * @property string $identifier
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property Carbon|null $deleted_at
 * @property string $source_id
 * @property-read Collection|Entry[] $entries
 * @property-read int|null $entries_count
 * @property-read Source|null $source
 * @property-read Collection|Vote[] $votes
 * @property-read int|null $votes_count
 * @method static Builder|Record newModelQuery()
 * @method static Builder|Record newQuery()
 * @method static Builder|Record onlyTrashed()
 * @method static Builder|Record query()
 * @method static Builder|Record whereAlias($value)
 * @method static Builder|Record whereCreatedAt($value)
 * @method static Builder|Record whereDeletedAt($value)
 * @method static Builder|Record whereId($value)
 * @method static Builder|Record whereIdentifier($value)
 * @method static Builder|Record whereSourceId($value)
 * @method static Builder|Record whereUpdatedAt($value)
 * @method static Builder|Record withTrashed()
 * @method static Builder|Record withoutTrashed()
 * @mixin Eloquent
 */
class Record extends Model
{
    use HasFactory, SoftDeletes;

    public static function store(Source|Builder $source): Record
    {
        $record             = new Record();
        $record->alias      = Alias::generate();
        $record->identifier = Identifier::infer();
        $record->source_id  = $source->id;
        $record->save();

        info('Registered new Record to the database.', [
            'record' => $record->id,
            'alias'  => $record->alias,
            'source' => $source->id
        ]);

        return $record;
    }

    public function export(): string
    {
        $result = '';

        foreach ($this->entries()->get() as $entry) {
            $result .= sprintf("%s | %s | %s | %s | %s | %s | %s\n",
                               $entry->number,
                               str_pad($entry->title, $this->pad('title')),
                               $entry->start,
                               $entry->end,
                               str_pad($entry->album, $this->pad('album')),
                               str_pad($entry->genre, $this->pad('genre')),
                               is_array($entry->artists)
                                   ? str_pad(implode(';', $entry->artists), $this->pad('artist'))
                                   : null

            );
        }

        return $result;
    }

    public function entries(): HasMany
    {
        return $this->hasMany(Entry::class);
    }

    private function pad($attribute)
    {
        return max(array_map('strlen', $this->entries->pluck($attribute)->toArray()));
    }

    public function getAliasAttribute($value): string
    {
        return sprintf("%s-%s", $this->attributes['alias'], $this->attributes['id']);
    }

    public function source(): BelongsTo
    {
        return $this->belongsTo(Source::class);
    }

    public function votes(): HasMany
    {
        return $this->hasMany(Vote::class);
    }
}
