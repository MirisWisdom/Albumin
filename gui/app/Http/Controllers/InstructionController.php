<?php

namespace App\Http\Controllers;

use App\Models\Instruction;
use App\Models\Record;

class InstructionController extends Controller
{
    /**
     * Display the specified resource.
     *
     * @param string $id
     * @return string[]
     */
    public function show(string $id): array
    {
        $explode = explode('-', $id);
        $alias   = sprintf("%s-%s", $explode[0], $explode[1]);
        $key     = $explode[2];
        $record  = Record::query()->where('id', $key)->where('alias', $alias)->first();

        return Instruction::generate($record);
    }
}
