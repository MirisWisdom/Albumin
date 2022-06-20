<?php

use App\Http\Controllers\EntryController;
use App\Http\Controllers\RecordController;
use App\Http\Controllers\SourceController;
use Illuminate\Support\Facades\Route;

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::get('/', function () {
    return view('welcome');
})->name('welcome');

Route::resource('sources', SourceController::class)->only(['store', 'show']);
Route::resource('sources.records', RecordController::class)->only(['store', 'show']);
Route::resource('sources.records.entries', EntryController::class)->only(['store', 'destroy']);
