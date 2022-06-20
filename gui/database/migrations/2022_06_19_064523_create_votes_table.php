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
        Schema::create('votes', function (Blueprint $table) {
            $table->id();
            $table->string('identifier');
            $table->timestamps();
            $table->softDeletes();
            $table->bigInteger('record_id')->unsigned();
        });

        Schema::table('votes', function (Blueprint $table) {
            $table->foreign('record_id')->references('id')->on('records');
        });
    }
};
