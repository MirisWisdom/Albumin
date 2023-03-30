<form action="{{ route('sources.records.entries.store', [$source, $record]) }}"
      method="post">
    @csrf

    <div class='grid'>
        <div>
            <label>
                Track No.:
                <input type="text"
                       placeholder="e.g. 1"
                       name="number"
                       value="{{ $entries->count() + 1 }}"
                       required>
            </label>
        </div>
        <div>
            <label>
                Song Title:
                <input type="text"
                       placeholder="e.g. Hacking to the Gate"
                       name="title"
                       required>
            </label>
        </div>
        <div>
            <label>
                Start Time:
                <input type="text"
                       placeholder="00:08:18"
                       pattern="^(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)$"
                       name="start"
                       required
                       value="{{ $start_time }}">
            </label>
        </div>
        <div>
            <label>
                End Time:
                <input type="text"
                       placeholder="00:18:28"
                       pattern="^(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)$"
                       name="end"
                       required>
            </label>
        </div>
    </div>
    <div class="grid">
        <div>
            <label>
                Album:
                <input type="text"
                       placeholder="e.g. STEINS;GATE Original Soundtrack"
                       name="album">
            </label>
        </div>
        <div>
            <label>
                Genre:
                <input type="text"
                       placeholder="e.g. J-Pop"
                       name="genre">
            </label>
        </div>
        <div>
            <label>
                Artist(s) (use ; for multiple):
                <input type="text"
                       placeholder="e.g. Kanako Itō; Chiyomaru Shikura; Isoe Toshimichi"
                       name="artists">
            </label>
        </div>
    </div>
    <input type="submit"
           value="Add Track">
</form>
<hr>
<h2>
    Songs List
</h2>
@if($entries->count() > 0)
<table>
    <thead>
        <tr>
            <th>#</th>
            <th>Title</th>
            <th>Start Time</th>
            <th>End Time</th>
            <th>Album</th>
            <th>Genre</th>
            <th>Artist(s)</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach($entries as $entry)
        <tr>
            <td>{{ $entry->number }}</td>
            <td>{{ $entry->title }}</td>
            <td>{{ $entry->start }}</td>
            <td>{{ $entry->end }}</td>
            <td>{{ $entry->album ?? 'N/A' }}</td>
            <td>{{ $entry->genre ?? 'N/A' }}</td>
            <td>{{ is_array($entry->artists) ? implode(', ', $entry->artists) : 'N/A' }}</td>
            <td>
                @if($identifier == $entry->identifier)
                    <a href="javascript:;" onclick="document.getElementById('delete-{{ $entry->identifier }}-form').submit();">
                        Delete
                    </a>
                @endif
            </td>
            @if($identifier == $entry->identifier)
            <form action="{{ route('sources.records.entries.destroy', [$source, $record, $entry]) }}"
                method="post"
                id='delete-{{ $entry->identifier }}-form'>
                @method('delete')
                @csrf
            </form>
            @endif
        </tr>
        @endforeach
    </tbody>
</table>
@else
<p>
    No track entries found for this record, yet!
</p>
@endif