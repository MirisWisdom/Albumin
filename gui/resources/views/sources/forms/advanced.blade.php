<form action="{{ route('sources.records.entries.store', [$source, $record]) }}"
      method="post">
    @csrf
    <input type="hidden"
           name="mode"
           value="import">
    <article class="message is-info">
        <div class="message-header">
            <p>Manual Importing</p>
        </div>
        <div class="message-body">
            <div class="content">
                <p>
                    You can manually import songs from text, such as by copying and pasting the video's description!
                </p>
            </div>
            <div class="box is-family-monospace">
                <p>
                    <b>FORMAT</b>
                </p>
                <p>
                    # | title | start time (hh:mm:ss) | end time (hh:mm:ss) | album | genre | artists (use ; to separate)
                </p>
            </div>
            <div class="box is-family-monospace">
                <p>
                    <b>EXAMPLE</b>
                </p>
                <p>
                    1 | Rock Over Japan | 00:02:18 | 00:04:28 | HHH | J-Pop | Himari; Hibari; Hikari
                    <br>
                    2 | A Cruel Angel's | 00:04:28 | 00:08:28 \------------- optional -------------/
                </p>
            </div>
        </div>
    </article>
    <div class="field">
        <div class="control">
            <label class="label"
                   for="import-textarea">
                Import New Songs:
            </label>
            <textarea class="textarea is-family-monospace"
                      id="import-textarea"
                      name="import"
                      placeholder="# | title | start time | end time | ...">{{ $export }}</textarea>
        </div>
    </div>

    <div class="field is-expanded">
        <input type="submit"
               class="button is-link is-large is-fullwidth"
               value="Import Songs">
    </div>
</form>
