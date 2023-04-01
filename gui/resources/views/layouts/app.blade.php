<!doctype html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport"
          content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible"
          content="ie=edge">
    <title>{{ config('app.name') }} ~ {{ config('app.author') }}</title>

    <meta property="og:title"
          content="{{config('app.name') }} ~ {{ config('app.author') }}">
    <meta property="og:type"
          content="website" />
    <meta property="og:image"
          content="{{ asset('apple-touch-icon.png') }}">
    <meta property="og:url"
          content="{{ route('welcome') }}">
    <meta name="twitter:card"
          content="Split YouTube videos into audio tracks.">

    <meta property="og:description"
          content="Split YouTube videos into audio tracks.">
    <meta property="og:site_name"
          content="{{ config('app.name') }} ~ {{ config('app.author') }}">
    <meta name="twitter:image:alt"
          content="Screenshot of Gunloader Website">

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
    <main class="container">
        <nav>
            <ul>
                <li>
                    <strong>{{ config('app.name') }}</strong>
                    - YouTube Album Splitter
                </li>
            </ul>
            <ul>
                <li><a href="{{ route('welcome') }}">Home</a></li>
                <li><a href="{{ route('sources.index') }}"
                       role='button'>All Videos</a></li>
            </ul>
        </nav>

        @if(session('success'))
        <article>
            <header>
                <p>Success</p>
            </div>
            {{ session('success') }}
        </article>
        @endif

        @if(session('error'))
        <article>
            <header>
                <p>Error</p>
            </div>
            {{ session('error') }}
        </article>
        @endif
    </main>

    @yield('content')

    <div class="container">
        <footer>
            <p>
                Project by
                <a href="https://github.com/MirisWisdom"
                   target="-_blank">
                    Miris Wisdom
                </a>
                available on
                <a href="https://github.com/MirisWisdom/gunloader"
                   target="_blank">
                    GitHub
                </a> =)
            </p>
        </footer>
    </div>
    </main>
</body>
<script>
    /*
    * Modal
    *
    * Pico.css - https://picocss.com
    * Copyright 2019-2023 - Licensed under MIT
    */

    // Config
    const isOpenClass = 'modal-is-open';
    const openingClass = 'modal-is-opening';
    const closingClass = 'modal-is-closing';
    const animationDuration = 400; // ms
    let visibleModal = null;


    // Toggle modal
    const toggleModal = event => {
    event.preventDefault();
    const modal = document.getElementById(event.currentTarget.getAttribute('data-target'));
    (typeof(modal) != 'undefined' && modal != null)
        && isModalOpen(modal) ? closeModal(modal) : openModal(modal)
    }

    // Is modal open
    const isModalOpen = modal => {
    return modal.hasAttribute('open') && modal.getAttribute('open') != 'false' ? true : false;
    }

    // Open modal
    const openModal = modal => {
    if (isScrollbarVisible()) {
        document.documentElement.style.setProperty('--scrollbar-width', `${getScrollbarWidth()}px`);
    }
    document.documentElement.classList.add(isOpenClass, openingClass);
    setTimeout(() => {
        visibleModal = modal;
        document.documentElement.classList.remove(openingClass);
    }, animationDuration);
    modal.setAttribute('open', true);
    }

    // Close modal
    const closeModal = modal => {
    visibleModal = null;
    document.documentElement.classList.add(closingClass);
    setTimeout(() => {
        document.documentElement.classList.remove(closingClass, isOpenClass);
        document.documentElement.style.removeProperty('--scrollbar-width');
        modal.removeAttribute('open');
    }, animationDuration);
    }

    // Close with a click outside
    document.addEventListener('click', event => {
    if (visibleModal != null) {
        const modalContent = visibleModal.querySelector('article');
        const isClickInside = modalContent.contains(event.target);
        !isClickInside && closeModal(visibleModal);
    }
    });

    // Close with Esc key
    document.addEventListener('keydown', event => {
    if (event.key === 'Escape' && visibleModal != null) {
        closeModal(visibleModal);
    }
    });

    // Get scrollbar width
    const getScrollbarWidth = () => {

    // Creating invisible container
    const outer = document.createElement('div');
    outer.style.visibility = 'hidden';
    outer.style.overflow = 'scroll'; // forcing scrollbar to appear
    outer.style.msOverflowStyle = 'scrollbar'; // needed for WinJS apps
    document.body.appendChild(outer);

    // Creating inner element and placing it in the container
    const inner = document.createElement('div');
    outer.appendChild(inner);

    // Calculating difference between container's full width and the child width
    const scrollbarWidth = (outer.offsetWidth - inner.offsetWidth);

    // Removing temporary elements from the DOM
    outer.parentNode.removeChild(outer);

    return scrollbarWidth;
    }

    // Is scrollbar visible
    const isScrollbarVisible = () => {
    return document.body.scrollHeight > screen.height;
    }
</script>

</html>