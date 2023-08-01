@extends('layouts.app')

@section('content')
<div class="container">
    <div class="grid">
        <div>
            <article>
                <header>
                    Prepare Video
                </header>
                <form action="{{ route('sources.store') }}"
                      method="post">
                    @csrf
                    <label>
                        YouTube URL:
                        <input type="text"
                               placeholder="https://youtu.be/rTmyZdWqt9Y"
                               name="source"
                               required>
                    </label>
                    <input type='submit'>
                    <hr>
                    <p>
                        Tip: The video should be a compilation of different songs.
                        <a href="https://youtu.be/n61ULEU7CO0"
                           target="_blank">This compilation</a>
                        is an example!
                    </p>
                </form>
            </article>
        </div>
        <div>
            <article>
                <header>
                    Welcome!
                </header>
                <p>
                    This website allows you to generate instructions for
                    <a href="https://github.com/MirisWisdom/albumin"
                       target="_blank">Albumin</a>,
                    including the track titles, timings, and album/artist/genre information.
                </p>
                <p>
                    To get started, specify the YouTube URL for a songs compilation using the form on this page!
                </p>
                <p>
                    You should
                    <a href="https://github.com/MirisWisdom/Albumin/releases"
                       target="_blank">download the Albumin program</a>,
                    along with the
                    <a href="https://github.com/MirisWisdom/Albumin#dependencies"
                       target="_blank">relevant dependencies</a>,
                    if you haven't done so already! =)
                </p>
            </article>
        </div>
    </div>
</div>
@endsection