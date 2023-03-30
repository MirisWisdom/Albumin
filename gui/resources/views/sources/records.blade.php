@extends('layouts.app')

@section('content')
<div class="container">
    @if($entries->count() > 0)
    <article>
        <label>
            Run the
            <a href="https://github.com/yumiris/Gunloader/releases/"
            target="_blank">
                Gunloader CLI
            </a>
            and enter the following ID...
        </label>
        <hr>
        <div class="grid">
            <div>
                <input type="text"
                    readonly
                    value="{{ $record->alias }}">
            </div>
            <div>
                <a href="https://github.com/yumiris/Gunloader/releases/"
                role="button"
                target="_blank"
                style="width: 100%">
                    Download Gunloader
                </a>
            </div>
            <div>
                <a href="{{ 'https://youtu.be/' . $record->source_id }}"
                role="button"
                class="outline"
                target="_blank"
                style="width: 100%">
                    View Video
                </a>
            </div>
            <div>
                <a href="{{ route('shell.show', $record->alias) }}"
                role="button"
                class="outline"
                target="_blank"
                style="width: 100%">
                    ffmpeg/yt-dlp
                </a>
            </div>
        </div>
    </article>
    @endif
    <article>
        <header>
            <h1 class="title">
                {{ $source->title }}
            </h1>
            <nav>
                <ul>
                    <li>
                        <a href="{{ route('sources.records.show', [$source, $record]) }}"
                           role="button"
                           {!! $advanced_mode ? "class='outline'" : null !!}>
                            Simple
                        </a>
                    </li>
                    <li>
                        <a href="{{ route('sources.records.show', [$source, $record, 'mode' => 'advanced']) }}"
                           role="button"
                           {!! $advanced_mode ? null : "class='outline'" !!}>
                            Advanced
                        </a>
                    </li>
                </ul>
            </nav>
        </header>
        @if($advanced_mode)
        @include('sources.forms.advanced')
        @else
        @include('sources.forms.simple')
        @endif
    </article>
</div>
<div class="container-fluid">
    <article>
        <div class="grid">
            <div>
                <article>
                    <div class="video-container">
                        <iframe src="https://www.youtube.com/embed/{{ $source->id }}?start={{ $embed_time }}"></iframe>
                    </div>
                </article>
            </div>
            <div>
                <article>
                    <header>
                        <h1>Video Description</h1>
                    </header>
                    <label>
                        The video's description <i>might</i> contain the titles and timings!
                        <hr>
                        <textarea style='font-family: monospace'
                                  rows="10"
                                  readonly>{{ $source->description }}</textarea>
                    </label>
                </article>
            </div>
        </div>
    </article>
</div>
@endsection