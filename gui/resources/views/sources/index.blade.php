@extends('layouts.app')

@section('content')
<div class="container-fluid">
    @foreach ($sources->chunk(4) as $chunk)
    <div class="grid">
        @foreach ($chunk as $source)
        <div>
            <article>
                <div class="video-container">
                    <iframe src="https://www.youtube.com/embed/{{ $source->id }}"></iframe>
                </div>
                <hr>
                <button data-target="modal-{{ $source->id }}"
                        onClick="toggleModal(event)">
                    Download Audio Tracks
                </button>
            </article>
        </div>

        <dialog id="modal-{{ $source->id }}">
            <article>
                <header>
                    <div class="video-container">
                        <iframe src="https://www.youtube.com/embed/{{ $source->id }}"></iframe>
                    </div>
                    <hr>
                    <h3>
                        {{ $source->title }}
                    </h3>
                    <hr>
                    <p>
                        {{ $source->records()->has('entries')->count() }} downloadable records
                    </p>
                </header>
                <label>
                    Run the
                    <a href="https://github.com/MirisWisdom/Gunloader/releases/"
                       target="_blank">
                        Gunloader CLI
                    </a>
                    and enter the following ID:
                    <input type="text"
                           readonly
                           value="{{ $source->record->alias }}"
                           style="text-align: center">
                </label>
                <div class="grid">
                    <div>
                        <a href="{{ route('sources.show', $source) }}"
                           role="button"
                           style="width: 100%">
                            View All Records
                        </a>
                    </div>
                    <div>
                        <a href="https://youtu.be/{{ $source->id }}"
                           role="button"
                           target="_blank"
                           style="width: 100%"
                           class="outline">
                            Watch Video
                        </a>
                    </div>
                </div>
                <footer>
                    <a href="#cancel"
                       role="button"
                       class="secondary"
                       data-target="modal-{{ $source->id }}"
                       onClick="toggleModal(event)">
                        Close
                    </a>
                </footer>
            </article>
        </dialog>
        @endforeach
    </div>
    @endforeach
</div>
@endsection