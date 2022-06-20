<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport"
          content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible"
          content="ie=edge">
    <title>{{ config('app.name') }}</title>
    <link rel="stylesheet"
          href="{{ asset('css/app.css') }}">
</head>
<body>
<div class="container">
    <section class="hero">
        <div class="hero-body">
            <p class="title">
                <a href="{{ route('welcome') }}">{{ config('app.name') }}</a>
            </p>
            <p class="subtitle">
                Split YouTube videos into audio tracks.
            </p>
        </div>
    </section>
    @if(session('success'))
        <article class="message is-success">
            <div class="message-header">
                <p>Success</p>
            </div>
            <div class="message-body">
                {{ session('success') }}
            </div>
        </article>
    @endif
    @if(session('error'))
        <article class="message is-danger">
            <div class="message-header">
                <p>Error</p>
            </div>
            <div class="message-body">
                {{ session('error') }}
            </div>
        </article>
    @endif
    @yield('content')
</div>
<hr>
<footer class="footer">
    <div class="content has-text-centered">
        <p>
            Project by
            <a href="https://github.com/yumiris"
               target="-_blank">
                Miris Wisdom
            </a>
            available on
            <a href="https://github.com/yumiris/gunloader"
               target="_blank">
                GitHub
            </a> =)
        </p>
    </div>
</footer>
</body>
</html>
