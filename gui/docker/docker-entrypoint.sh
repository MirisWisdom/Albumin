#!/bin/sh
set -e

if [ "$1" = 'frankenphp' ] || [ "$1" = 'php' ] || [ "$1" = 'artisan' ]; then
	php artisan optimize --no-interaction
	php artisan storage:link --no-interaction --force

	sqlite3 storage/database.sqlite 'pragma journal_mode=wal'

	echo "Waiting for database to be ready..."
	ATTEMPTS_LEFT_TO_REACH_DATABASE=60
	until [ $ATTEMPTS_LEFT_TO_REACH_DATABASE -eq 0 ] || DATABASE_ERROR=$(php artisan db:show 2>&1); do
		if [ $? -eq 255 ]; then
			# If the Doctrine command exits with 255, an unrecoverable error occurred
			ATTEMPTS_LEFT_TO_REACH_DATABASE=0
			break
		fi
		sleep 1
		ATTEMPTS_LEFT_TO_REACH_DATABASE=$((ATTEMPTS_LEFT_TO_REACH_DATABASE - 1))
		echo "Still waiting for database to be ready... Or maybe the database is not reachable. $ATTEMPTS_LEFT_TO_REACH_DATABASE attempts left."
	done

	if [ $ATTEMPTS_LEFT_TO_REACH_DATABASE -eq 0 ]; then
		echo "The database is not up or not reachable:"
		echo "$DATABASE_ERROR"
		exit 1
	else
		echo "The database is now ready and reachable"
	fi

	if [ "$( find ./database/migrations -iname '*.php' -print -quit )" ]; then
		php artisan migrate --no-interaction --isolated --force
	fi

	setfacl -R -m u:www-data:rwX -m u:"$(whoami)":rwX storage
	setfacl -dR -m u:www-data:rwX -m u:"$(whoami)":rwX storage
fi

exec docker-php-entrypoint "$@"
