@extends('layouts.app')

@section('content')
    <div class="columns">
        <div class="column">
            <div class="card">
                <div class="video-container">
                    <iframe src="https://www.youtube.com/embed/{{ $source->id }}"></iframe>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="card">
                <div class="card-content">
                    <h2 class="subtitle">
                        Available Instructions
                    </h2>
                    @if($records->count() > 0)
                        <div class="block">
                            <p>
                                The following existing instructions have been found for this video! Feel free to use
                                them or create your own! =)
                                <br><br>
                                To use them, paste their ID into Gunloader:
                            </p>
                        </div>
                        <table class="table is-fullwidth">
                            <thead>
                            <tr>
                                <th class="has-text-centered">Gunloader ID</th>
                                <th class="has-text-centered">Songs</th>
                                <th class="has-text-centered">Votes</th>
                                <th class="has-text-centered">Actions</th>
                            </tr>
                            </thead>
                            <tbody>
                            @foreach($records as $record)
                                <tr class="has-text-centered">
                                    <td>
                                        <div class="control">
                                            <input type="text"
                                                   value="{{ $record->alias }}"
                                                   readonly
                                                   class="input is-small is-family-monospace">
                                        </div>
                                    </td>
                                    <td>{{ $record->entries()->count() }}</td>
                                    <td>{{ $record->votes()->count() }}</td>
                                    <td>
                                        <a href="{{ route('sources.records.show', [$source, $record]) }}"
                                           class="button is-link is-small"
                                           target="_blank">
                                            View
                                        </a>
                                    </td>
                                </tr>
                            @endforeach
                            </tbody>
                        </table>
                    @else
                        <div class="content">
                            No instructions found for this video! Would you like to create one?
                        </div>
                    @endif
                    <form action="{{ route('sources.records.store', [$source]) }}"
                          method="post">
                        @csrf
                        <input type="hidden"
                               name="source">
                        <input type="submit"
                               class="button is-link is-fullwidth"
                               value="Create New Instructions">
                    </form>
                </div>
            </div>
        </div>
    </div>
@endsection
