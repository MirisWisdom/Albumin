#syntax=docker/dockerfile:1.4

# Versions
FROM dunglas/frankenphp:1-php8.3 AS albumin_upstream

# The different stages of this Dockerfile are meant to be built into separate images
# https://docs.docker.com/develop/develop-images/multistage-build/#stop-at-a-specific-build-stage
# https://docs.docker.com/compose/compose-file/#target


# Base Albumin image
FROM albumin_upstream AS albumin_base

WORKDIR /app

VOLUME /app/storage/

# persistent / runtime deps
# hadolint ignore=DL3008
RUN apt-get update && apt-get install -y --no-install-recommends \
	acl \
	file \
	gettext \
	git \
	mariadb-client \
	sqlite3 \
	&& rm -rf /var/lib/apt/lists/*

RUN set -eux; \
	install-php-extensions \
		@composer \
		apcu \
		intl \
		opcache \
		zip \
		pdo_mysql \
		pdo_sqlite \
	;

# https://getcomposer.org/doc/03-cli.md#composer-allow-superuser
ENV COMPOSER_ALLOW_SUPERUSER=1

COPY --link docker/app.ini $PHP_INI_DIR/conf.d/
COPY --link --chmod=755 docker/docker-entrypoint.sh /usr/local/bin/docker-entrypoint
COPY --link docker/Caddyfile /etc/caddy/Caddyfile

ENTRYPOINT ["docker-entrypoint"]

HEALTHCHECK --start-period=60s CMD curl -f http://localhost:80/up || exit 1
CMD [ "frankenphp", "run", "--config", "/etc/caddy/Caddyfile" ]

# Production Albumin image
FROM albumin_base AS albumin_prod

ENV APP_ENV=production

RUN mv "$PHP_INI_DIR/php.ini-production" "$PHP_INI_DIR/php.ini"

# prevent the reinstallation of vendors at every changes in the source code
COPY --link composer.* ./
RUN set -eux; \
	composer install --no-cache --prefer-dist --no-dev --no-autoloader --no-scripts --no-progress

# copy sources
COPY --link . ./
RUN rm -Rf docker/

RUN set -eux; \
mkdir -p \
	bootstrap/cache \
	storage/app/public \
	storage/framework/cache/data \
	storage/framework/sessions \
	storage/framework/testing \
	storage/framework/views \
	storage/logs; \
	composer dump-autoload --classmap-authoritative --no-dev; \
	chmod +x artisan; sync;

# Multi-stage build: Build static assets
# This allows us to not include Node within the final container
FROM node:18 as albumin_node

RUN mkdir -p /app
WORKDIR /app
COPY . .
COPY --from=albumin_prod /app/vendor /app/vendor

# Use yarn or npm depending on what type of
# lock file we might find. Defaults to
# NPM if no lock file is found.
# Note: We run "production" for Mix and "build" for Vite
RUN if [ -f "vite.config.js" ]; then \
		ASSET_CMD="build"; \
	else \
		ASSET_CMD="production"; \
	fi; \
	if [ -f "yarn.lock" ]; then \
		yarn install --frozen-lockfile; \
		yarn $ASSET_CMD; \
	elif [ -f "pnpm-lock.yaml" ]; then \
		corepack enable && corepack prepare pnpm@latest-8 --activate; \
		pnpm install --frozen-lockfile; \
		pnpm run $ASSET_CMD; \
	elif [ -f "package-lock.json" ]; then \
		npm ci --no-audit; \
		npm run $ASSET_CMD; \
	else \
		npm install; \
		npm run $ASSET_CMD; \
	fi;

# From our base container created above, we
# create our final image, adding in static
# assets that we generated above
FROM albumin_prod

# Packages like Laravel Nova may have added assets to the public directory
# or maybe some custom assets were added manually! Either way, we merge
# in the assets we generated above rather than overwrite them
COPY --from=albumin_node /app/public /app/public-npm
RUN cp -ar /app/public-npm/. /app/public/ \
	&& rm -rf /app/public-npm

EXPOSE 80

ARG APP_SOURCES
ENV APP_SOURCES=${APP_SOURCES}

ARG APP_VERSION
ENV APP_VERSION=${APP_VERSION}
