<?php

namespace App\Models;

use Alaouy\Youtube\Facades\Youtube;
use Barryvdh\LaravelIdeHelper\Eloquent;
use Exception;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Database\Eloquent\Collection;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Support\Carbon;
use Illuminate\Support\Facades\Log;
use InvalidArgumentException;

/**
 * App\Models\Source
 *
 * @property string $id
 * @property string $title
 * @property string $description
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property Carbon|null $deleted_at
 * @property-read Collection|Record[] $records
 * @property-read int|null $records_count
 * @method static Builder|Source newModelQuery()
 * @method static Builder|Source newQuery()
 * @method static Builder|Source onlyTrashed()
 * @method static Builder|Source query()
 * @method static Builder|Source whereCreatedAt($value)
 * @method static Builder|Source whereDeletedAt($value)
 * @method static Builder|Source whereId($value)
 * @method static Builder|Source whereTitle($value)
 * @method static Builder|Source whereDescription($value)
 * @method static Builder|Source whereUpdatedAt($value)
 * @method static Builder|Source withTrashed()
 * @method static Builder|Source withoutTrashed()
 * @mixin Eloquent
 */
class Source extends Model
{
    use HasFactory, SoftDeletes;

    public $incrementing = false;

    /**
     * Store a newly created resource in storage. Will return an existing record if a known ID is provided.
     *
     * @param string $url
     * @return Source
     */
    public static function store(string $url): Source
    {
        $id     = Source::parse($url);
        $source = Source::find($id);

        if ($source == null) {
            $source     = new Source();
            $source->id = $id;

            try {
                $video               = Youtube::getVideoInfo($id);
                $source->title       = $video->snippet->title;
                $source->description = $video->snippet->description;
            } catch (Exception $exception) {
                Log::error('Could not retrieve video information from YouTube API.', [
                    'source'    => $id,
                    'exception' => $exception->getTraceAsString()
                ]);
            }

            $source->save();

            info('Registered new Source to the database.', [
                'source' => $source->id
            ]);

            return $source;
        }

        return $source;
    }

    public static function parse(string $url): string
    {
        if (!str_contains($url, 'youtu') && strlen($url) != 11) {
            throw new InvalidArgumentException("Invalid data provided. Must be a YouTube URL or ID.");
        }

        return substr($url, -11);
    }

    public function records(): HasMany
    {
        return $this->hasMany(Record::class);
    }
}
