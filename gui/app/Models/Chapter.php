<?php

namespace App\Models;

use Barryvdh\LaravelIdeHelper\Eloquent;
use Exception;
use Illuminate\Database\Eloquent\Collection;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Database\Query\Builder;
use Illuminate\Support\Carbon;
use Illuminate\Support\Facades\Log;

/**
 * App\Models\Chapter
 *
 * @property int $id
 * @property string $title
 * @property string $start
 * @property string $end
 * @property Carbon|null $created_at
 * @property Carbon|null $updated_at
 * @property Carbon|null $deleted_at
 * @property string $source_id
 * @property-read Source|null $source
 * @method static Builder|Chapter newModelQuery()
 * @method static Builder|Chapter newQuery()
 * @method static Builder|Chapter onlyTrashed()
 * @method static Builder|Chapter query()
 * @method static Builder|Chapter whereCreatedAt($value)
 * @method static Builder|Chapter whereDeletedAt($value)
 * @method static Builder|Chapter whereEnd($value)
 * @method static Builder|Chapter whereId($value)
 * @method static Builder|Chapter whereSourceId($value)
 * @method static Builder|Chapter whereStart($value)
 * @method static Builder|Chapter whereTitle($value)
 * @method static Builder|Chapter whereUpdatedAt($value)
 * @method static Builder|Chapter withTrashed()
 * @method static Builder|Chapter withoutTrashed()
 * @mixin Eloquent
 */
class Chapter extends Model
{
    use SoftDeletes;

    /**
     * Stores Chapter records in the database for the given Source.
     *
     * Information will be retrieved from YouTube if available.
     *
     * @param Source $source
     * @return Collection|void
     */
    public static function cache(Source $source)
    {
        try {
            $command  = sprintf("%s | %s",
                                escapeshellcmd(sprintf("yt-dlp --dump-json '%s'", $source->id)),
                                "jq --raw-output '.chapters'");
            $chapters = json_decode(shell_exec($command));

            if ($chapters != null) {
                foreach ($chapters as $c) {
                    $chapter            = new Chapter();
                    $chapter->title     = $c->title;
                    $chapter->start     = Time::fromSeconds($c->start_time);
                    $chapter->end       = Time::fromSeconds($c->end_time);
                    $chapter->source_id = $source->id;
                    $chapter->save();

                    info('Registered new Chapter to the database.', [
                        'chapter' => $chapter->id,
                        'source'  => $source->id
                    ]);
                }

                return $source->chapters()->get();
            }
        } catch (Exception $exception) {
            Log::error('Error occurred when registering Chapter to the database.', [
                'source'  => $source->id,
                'chapter' => $exception->getTraceAsString()
            ]);
        }
    }

    public function source(): BelongsTo
    {
        return $this->belongsTo(Source::class);
    }
}
