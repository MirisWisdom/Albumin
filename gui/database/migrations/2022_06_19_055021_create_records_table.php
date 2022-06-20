<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('records', function (Blueprint $table) {
            $table->id();
            $table->string('alias')->index();
            $table->string('identifier');
            $table->timestamps();
            $table->softDeletes();
            $table->string('source_id');
        });

        Schema::table('records', function (Blueprint $table) {
            $table->foreign('source_id')->references('id')->on('sources');
        });
    }
};
