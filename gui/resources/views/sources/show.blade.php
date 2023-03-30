@extends('layouts.app')

@section('content')
<div class="container">
    <header>
        <h1>
            {{ $source->title }}
        </h1>
    </header>
</div>
<div class="container-fluid">
    <div class="grid">
        <div>
            <article>
                <div class="card-content">
                    <h2 class="subtitle">
                        Available Records
                    </h2>
                    @if($records->count() > 0)
                    <p>
                        The following existing records have been found for this video! Feel free to use them or
                        create your own! =)
                        <br><br>
                        To use them, paste their ID into Gunloader:
                    </p>
                    <table>
                        <thead>
                            <tr>
                                <th style="text-align: center">Gunloader ID</th>
                                <th style="text-align: center">Songs</th>
                                <th style="text-align: center">Votes</th>
                                <th style="text-align: center">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach($records as $record)
                            <tr>
                                <td style="text-align: center">
                                    <input type="text"
                                           value="{{ $record->alias }}"
                                           readonly>
                                </td>
                                <td style="text-align: center">{{ $record->entries()->count() }}</td>
                                <td style="text-align: center">{{ $record->votes()->count() }}</td>
                                <td style="text-align: center">
                                    <a href="{{ route('sources.records.show', [$source, $record]) }}"
                                       role="button"
                                       target="_blank"
                                       class="outline">
                                        View
                                    </a>
                                </td>
                            </tr>
                            @endforeach
                        </tbody>
                    </table>
                    @else
                    <div class="content">
                        No records found for this video! Would you like to create one?
                    </div>
                    @endif
                    <form action="{{ route('sources.records.store', [$source]) }}"
                          method="post">
                        @csrf
                        <input type="hidden"
                               name="source">
                        <input type="submit"
                               value="Create New Records"
                               style="width: 100%">
                    </form>
                </div>
            </article>
        </div>
        <div>
            <article>
                <div class="video-container">
                    <iframe src="https://www.youtube.com/embed/{{ $source->id }}"></iframe>
                </div>
            </article>
        </div>
    </div>
</div>
@endsection