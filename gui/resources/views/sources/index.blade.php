@extends('layouts.app')

@section('content')
    @foreach ($sources->chunk(3) as $chunk)
        <div class="columns">
            @foreach ($chunk as $source)
                <div class="column">
                    <div class="card">
                        <div class="card-image">
                            <figure class="image is-4by3">
                                <img src="https://i.ytimg.com/vi/{{ $source->id }}/hqdefault.jpg"
                                     alt="Thumbnail for {{ $source->title }}">
                            </figure>
                        </div>
                        <div class="card-content">
                            <div class="media">
                                <div class="media-left">
                                    <figure class="image is-48x48">
                                        <img src="https://i.ytimg.com/vi/{{ $source->id }}/3.jpg"
                                             alt="Placeholder image">
                                    </figure>
                                </div>
                                <div class="media-content">
                                    <p class="title is-4">
                                        {{ $source->title }}
                                    </p>
                                    <p class="subtitle is-6">
                                        {{ $source->records()->has('entries')->count() }} Records
                                    </p>
                                </div>
                            </div>
                            <div class="content">
                                {{ \Illuminate\Support\Str::limit($source->description, 72) }}
                                <hr>
                                <a href="{{ route('sources.show', $source) }}"
                                   class="button is-link">
                                    View Records
                                </a>
                                <a href="https://youtu.be/{{ $source->id }}"
                                   class="button is-outlined is-link"
                                   target="_blank">
                                    Watch Video
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            @endforeach
        </div>
    @endforeach
@endsection
