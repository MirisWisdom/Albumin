<?php

namespace App\Http\Controllers;

use App\Models\FFmpeg;
use App\Models\Record;

class FFmpegController extends Controller
{
    public function show(string $id): string
    {
        return FFmpeg::generate(Record::infer($id));
    }
}
