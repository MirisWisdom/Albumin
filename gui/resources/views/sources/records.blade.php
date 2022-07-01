@extends('layouts.app')

@section('content')
    @if($entries->count() > 0)
        <label class="label"
               for="url-text">
            Run the
            <a href="https://github.com/yumiris/Gunloader/releases/"
               target="_blank">
                Gunloader CLI
            </a>
            and enter the following ID:
        </label>

        <div class="columns">
            <div class="column is-one-third">
                <div class="field has-addons">
                    <div class="control is-expanded">
                        <input class="input is-large is-family-monospace has-text-centered"
                               type="text"
                               id="url-text"
                               readonly
                               value="{{ $record->alias }}">
                    </div>
                </div>
            </div>
            <div class="column is-one-third">
                <div class="control">
                    <a href="https://github.com/yumiris/Gunloader/releases/"
                       class="button is-link is-large is-fullwidth"
                       target="_blank">
                        Download Gunloader
                    </a>
                </div>
            </div>
            <div class="column">
                <div class="control">
                    <a href="{{ 'https://youtu.be/' . $record->source_id }}"
                       class="button is-outlined is-link is-large is-fullwidth"
                       target="_blank">
                        View Video
                    </a>
                </div>
            </div>
            <div class="column">
                <div class="control">
                    <a href="{{ route('ffmpeg.show', $record->alias) }}"
                       class="button is-outlined is-link is-large is-fullwidth"
                       target="_blank">
                        Shell Script
                    </a>
                </div>
            </div>
        </div>
    @endif
    <div class="box has-text-centered">
        <h1 class="title">{{ $source->title }}</h1>
    </div>
    <div class="block">
        <div class="card">
            <div class="card-content">
                <div class="tabs">
                    <ul>
                        <li {!! $advanced_mode ? '' : "class='is-active'" !!}>
                            <a href="{{ route('sources.records.show', [$source, $record]) }}">
                                Simple
                            </a>
                        </li>
                        <li {!! $advanced_mode ? "class='is-active'" : '' !!}>
                            <a href="{{ route('sources.records.show', [$source, $record, 'mode' => 'advanced']) }}">
                                Advanced
                            </a>
                        </li>
                    </ul>
                </div>
                @if($advanced_mode)
                    @include('sources.forms.advanced')
                @else
                    @include('sources.forms.simple')
                @endif
            </div>
        </div>
    </div>
    <div class="block">
        <div class="card">
            <div class="card-content">
                <div class="columns">
                    <div class="column">
                        <div class="video-container">
                            <iframe src="https://www.youtube.com/embed/{{ $source->id }}?start={{ $embed_time }}"></iframe>
                        </div>
                    </div>
                    <div class="column">
                        <article class="message is-info">
                            <div class="message-header">
                                <p>Video Description</p>
                            </div>
                            <div class="message-body">
                                <label class="label"
                                       for="reference-textarea">
                                    The video's description <i>might</i> contain the titles and timings!
                                </label>
                                <textarea class="textarea is-family-monospace"
                                          id="reference-textarea"
                                          rows="10"
                                          readonly>{{ $source->description }}</textarea>
                            </div>
                        </article>
                    </div>
                </div>

            </div>
        </div>
    </div>
@endsection
