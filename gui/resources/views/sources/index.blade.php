@extends('layouts.app')

@section('content')
    @foreach ($sources->chunk(3) as $chunk)
        <div class="columns">
            @foreach ($chunk as $source)
                <div class="column">
                    <div class="card">
                        <div class="card-image">
                            <div class="video-container">
                                <iframe src="https://www.youtube.com/embed/{{ $source->id }}"></iframe>
                            </div>

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
                                               value="{{ $source->record->alias }}">
                                    </div>
                                </div>
                                <div class="columns">
                                    <div class="column">
                                        <a href="{{ route('sources.show', $source) }}"
                                           class="button is-link is-fullwidth">
                                            View All Records
                                        </a>
                                    </div>
                                    <div class="column">
                                        <a href="https://youtu.be/{{ $source->id }}"
                                           class="button is-outlined is-link is-fullwidth"
                                           target="_blank">
                                            Watch Video
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            @endforeach
        </div>
    @endforeach
@endsection
