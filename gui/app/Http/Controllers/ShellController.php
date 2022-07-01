<?php

namespace App\Http\Controllers;

use App\Models\Record;
use App\Models\Shell;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\Routing\ResponseFactory;
use Illuminate\Http\Response;

class ShellController extends Controller
{
    public function show(string $id): Response|Application|ResponseFactory
    {
        return response(Shell::generate(Record::infer($id)), 200)
            ->header('Content-Type', 'text/plain');
    }
}
