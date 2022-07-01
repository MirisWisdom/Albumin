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
        return Instruction::generate(Record::infer($id));
    }
}
