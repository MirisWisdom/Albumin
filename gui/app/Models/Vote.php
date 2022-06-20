<?php

namespace App\Models;

use Barryvdh\LaravelIdeHelper\Eloquent;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Database\Query\Builder;
use Illuminate\Support\Carbon;

/**
 * App\Models\Vote
 *
 * @property int $id
 * @property string $identifier
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property Carbon|null $deleted_at
 * @property int $record_id
 * @property-read Record|null $record
 * @method static Builder|Vote newModelQuery()
 * @method static Builder|Vote newQuery()
 * @method static Builder|Vote onlyTrashed()
 * @method static Builder|Vote query()
 * @method static Builder|Vote whereCreatedAt($value)
 * @method static Builder|Vote whereDeletedAt($value)
 * @method static Builder|Vote whereId($value)
 * @method static Builder|Vote whereIdentifier($value)
 * @method static Builder|Vote whereRecordId($value)
 * @method static Builder|Vote whereUpdatedAt($value)
 * @method static Builder|Vote withTrashed()
 * @method static Builder|Vote withoutTrashed()
 * @mixin Eloquent
 */
class Vote extends Model
{
    use HasFactory, SoftDeletes;

    public function record(): BelongsTo
    {
        return $this->belongsTo(Record::class);
    }
}
