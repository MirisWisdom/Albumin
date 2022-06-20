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
    <link rel="apple-touch-icon"
          sizes="180x180"
          href="{{ asset('apple-touch-icon.png') }}">
    <link rel="icon"
          type="image/png"
          sizes="32x32"
          href="{{ asset('favicon-32x32.png') }}">
    <link rel="icon"
          type="image/png"
          sizes="16x16"
          href="{{ asset('favicon-16x16.png') }}">
    <link rel="manifest"
          href="{{ asset('site.webmanifest') }}">
</head>
<body>
<div class="container">
    <section class="hero has-text-centered">
        <div class="hero-body">
            <img src="{{ asset('apple-touch-icon.png') }}"
                 alt="Gunloader icon">
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
