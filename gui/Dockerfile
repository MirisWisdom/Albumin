FROM ghcr.io/linuxserver/nginx:1.22.1

ARG COMPOSER_ALLOW_SUPERUSER=1

WORKDIR /app/www/

RUN apk add --no-cache \
    composer \
    npm

COPY ./composer.lock        /app/www/composer.lock
COPY ./composer.json        /app/www/composer.json

RUN composer install \
	--prefer-dist \
	--no-scripts \
	--no-dev \
	--no-autoloader

COPY ./webpack.mix.js       /app/www/webpack.mix.js
COPY ./package.json         /app/www/package.json
COPY ./package-lock.json    /app/www/package-lock.json
COPY ./resources/js         /app/www/resources/js
COPY ./resources/css        /app/www/resources/css

RUN npm install
RUN npm run prod
RUN rm -rf node_modules

COPY ./                     /app/www

RUN composer dump-autoload -o

FROM ghcr.io/linuxserver/nginx:1.22.1

RUN apk add --no-cache \
    sqlite

COPY --chown=abc:users --from=0 /app/www/docker /
COPY --chown=abc:users --from=0 /app/www        /app/www
