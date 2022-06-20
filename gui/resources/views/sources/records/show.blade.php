@extends('layouts.app')

@section('content')
    @if($entries->count() > 0)
        <div class="content">
            <label class="label"
                   for="url-text">
                Run the
                <a href="https://github.com/yumiris/Gunloader/releases/"
                   target="_blank">
                    Gunloader CLI
                </a>
                and enter the following ID:
            </label>
            <div class="field has-addons">
                <div class="control is-expanded">
                    <input class="input is-large is-family-monospace has-text-centered"
                           type="text"
                           id="url-text"
                           readonly
                           value="{{ $record->alias }}">
                </div>
                <div class="control">
                    <a href="{{ route('instructions.show', $record->alias) }}"
                       class="button is-outlined is-link is-large"
                       target="_blank">
                        Inspect
                    </a>
                </div>
            </div>
        </div>
    @endif
    <div class="card">
        <div class="card-content">
            <h2 class="subtitle">
                Available Tracks
            </h2>
            @if($entries->count() > 0)
                <table class="table is-fullwidth">
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
                            <td>{{ implode(', ', $entry->artists) }}</td>
                            <td>
                                <form action="{{ route('sources.records.entries.destroy', [$source, $record, $entry]) }}"
                                      method="post">
                                    @method('delete')
                                    @csrf
                                    <input type="submit"
                                           class="button is-small is-danger"
                                           value="Delete">
                                </form>
                            </td>
                        </tr>
                    @endforeach
                    </tbody>
                </table>
            @else
                <div class="content">
                    No track entries found for this record, yet!
                </div>
            @endif
            <hr>
            <form action="{{ route('sources.records.entries.store', [$source, $record]) }}"
                  method="post">
                @csrf
                <div class="field has-addons is-fullwidth">
                    <div class="control"
                         style="max-width: 150px;">
                        <label for="number-text">
                            Track No.:
                        </label>
                        <input class="input is-large"
                               type="text"
                               placeholder="e.g. 1"
                               id="number-text"
                               name="number"
                               value="{{ $entries->count() + 1 }}"
                               required>
                    </div>
                    <div class="control is-expanded">
                        <label for="title-text">
                            Song Title:
                        </label>
                        <input class="input is-large"
                               type="text"
                               placeholder="e.g. Hacking to the Gate"
                               id="title-text"
                               name="title"
                               required>
                    </div>
                    <div class="control"
                         style="max-width: 250px;">
                        <label for="start-text">
                            Start Time:
                        </label>
                        <input class="input is-large"
                               type="text"
                               placeholder="00:08:18"
                               pattern="^(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)$"
                               id="start-text"
                               name="start"
                               required>
                    </div>
                    <div class="control"
                         style="max-width: 250px;">
                        <label for="end-text">
                            End Time:
                        </label>
                        <input class="input is-large"
                               type="text"
                               placeholder="00:18:28"
                               pattern="^(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)$"
                               id="end-text"
                               name="end"
                               required>
                    </div>
                </div>

                <div class="field has-addons is-fullwidth">
                    <div class="control">
                        <label for="album-text">
                            Album:
                        </label>
                        <input class="input"
                               type="text"
                               placeholder="e.g. STEINS;GATE Original Soundtrack"
                               id="album-text"
                               name="album">
                    </div>
                    <div class="control">
                        <label for="genre-text">
                            Genre:
                        </label>
                        <input class="input"
                               type="text"
                               placeholder="e.g. J-Pop"
                               id="genre-text"
                               name="genre">
                    </div>
                    <div class="control is-expanded">
                        <label for="artists-text">
                            Artist(s) (use ; to separate multiple artists):
                        </label>
                        <input class="input"
                               type="text"
                               placeholder="e.g. Kanako Itō; Chiyomaru Shikura; Isoe Toshimichi"
                               id="artists-text"
                               name="artists">
                    </div>
                </div>

                <div class="field is-expanded">
                    <input type="submit"
                           class="button is-link is-large is-fullwidth"
                           value="Add Track">
                </div>
            </form>
        </div>
    </div>
    <div class="card">
        <div class="video-container">
            <iframe src="https://www.youtube.com/embed/{{ $source->id }}"></iframe>
        </div>
    </div>
@endsection
