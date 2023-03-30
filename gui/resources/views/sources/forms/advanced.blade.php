<form action="{{ route('sources.records.entries.store', [$source, $record]) }}"
      method="post">
    @csrf
    <input type="hidden"
           name="mode"
           value="import">
    <p>
        You can manually import songs from text, such as by copying and pasting the video's description!
    </p>
    <p style="font-family: monospace">
        <b>FORMAT</b>
        <br>
        # | title | start time (hh:mm:ss) | end time (hh:mm:ss) | album | genre | artists
        <br>
        <br>
        <b>EXAMPLE</b>
        <br>
        1 | Rock Over Japan | 00:02:18 | 00:04:28 | HHH | J-Pop | Himari; Hibari; Hikari
        <br>
        2 | A Cruel Angel's | 00:04:28 | 00:08:28 \------------- optional -------------/
    </p>
    <label class="label">
        Import New Songs:
        <textarea style="font-family: monospace"
                  name="import"
                  placeholder="# | title | start time | end time | ...">{{ $export }}</textarea>
    </label>
    <input type="submit"
           value="Import Songs">
    </div>
</form>