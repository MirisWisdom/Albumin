@extends('layouts.app')

@section('content')
    <div class="columns">
        <div class="column">
            <div class="card">
                <div class="card-content">
                    <h2 class="subtitle">
                        Welcome!
                    </h2>
                    <p>
                        This website allows you to generate instructions for
                        <a href="https://github.com/yumiris/gunloader"
                           target="_blank">Gunloader</a>,
                        including the track titles, timings, and album/artist/genre information.
                        <br><br>
                        To get started, specify the YouTube URL for a songs compilation using the form on this page!
                        <br><br>
                        You should
                        <a href="https://github.com/yumiris/Gunloader/releases"
                           target="_blank">download the Gunloader program</a>,
                        along with the
                        <a href="https://github.com/yumiris/Gunloader#dependencies"
                           target="_blank">relevant dependencies</a>,
                        if you haven't done so already! =)
                    </p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="card">
                <div class="card-content">
                    <h2 class="subtitle">
                        Prepare Video
                    </h2>
                    <form action="{{ route('sources.store') }}"
                          method="post">
                        @csrf
                        <label class="label"
                               for="source-text">
                            YouTube URL:
                        </label>
                        <div class="field has-addons">
                            <div class="control is-expanded">
                                <input class="input is-fullwidth is-large"
                                       type="text"
                                       placeholder="https://youtu.be/rTmyZdWqt9Y"
                                       id="source-text"
                                       name="source">
                            </div>
                            <div class="control">
                                <button class="button is-link is-large">
                                    Submit
                                </button>
                            </div>
                        </div>
                        <hr>
                        <p>
                            Tip: The video should be a compilation of different songs.
                            <a href="https://youtu.be/n61ULEU7CO0"
                               target="_blank">This compilation</a>
                            is an example of what videos we refer to!
                        </p>
                    </form>
                </div>
            </div>
        </div>
    </div>
@endsection
