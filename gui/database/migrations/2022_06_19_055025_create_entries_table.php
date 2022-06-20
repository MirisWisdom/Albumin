<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('entries', function (Blueprint $table) {
            $table->id();
            $table->integer('number')->unsigned();
            $table->string('title');
            $table->string('start');
            $table->string('end');
            $table->string('album')->nullable();
            $table->string('genre')->nullable();
            $table->json('artists')->nullable();
            $table->string('identifier');
            $table->timestamps();
            $table->softDeletes();
            $table->bigInteger('record_id')->unsigned();
        });

        Schema::table('entries', function (Blueprint $table) {
            $table->foreign('record_id')->references('id')->on('records');
        });
    }
};
